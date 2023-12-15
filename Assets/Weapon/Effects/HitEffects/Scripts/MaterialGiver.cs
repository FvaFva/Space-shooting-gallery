using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialGiver
{
    private List<Material> _materials;

    public List<Material> GetMaterials(RaycastHit hitInfo)
    {
        _materials = new List<Material>();

        SearchInMesh(hitInfo);
        SearchInAnimator(hitInfo);

        return _materials;
    }

    private void SearchInMesh(RaycastHit hitInfo)
    {
        MeshRenderer targetMesh = hitInfo.collider.GetComponent<MeshRenderer>();

        if (targetMesh == null)
        {
            var parentsMeshes = hitInfo.collider.GetComponentsInParent<MeshRenderer>();
            var meshRenderers = parentsMeshes.Concat(hitInfo.collider.GetComponentsInChildren<MeshRenderer>()).Distinct();

            if (meshRenderers.Count() == 0)
                return;

            foreach (MeshRenderer meshRenderer in meshRenderers)
                AddMaterialsFromMeshRenderer(meshRenderer, hitInfo.triangleIndex);
        }
        else
        {
            AddMaterialsFromMeshRenderer(targetMesh, hitInfo.triangleIndex);
        }
    }

    private void AddMaterialsFromMeshRenderer(MeshRenderer renderer, int triangleIndex)
    {
        if (renderer.materials.Length == 1)
        {
            _materials.Add(renderer.material);
        }
        else
        {
            Mesh mesh = renderer.additionalVertexStreams ?? renderer.GetComponent<MeshFilter>()?.sharedMesh;

            if (mesh == null)
                return;

            int materialIndex = -1;
            int[] allTriangles = mesh.triangles;

            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                int[] triangles = mesh.GetTriangles(i);

                if (IsContainsTriangle(triangles, triangleIndex, allTriangles))
                {
                    materialIndex = i;
                    break;
                }

            }

            if (materialIndex == -1)
                return;

            _materials.Add(renderer.materials[materialIndex]);
        }
    }

    private bool IsContainsTriangle(int[] subMeshTriangles, int triangleIndex, int[] allTriangles)
    {
        int startIndex = triangleIndex * 3;

        for (int i = 0; i< 3; i++)
        {
            if (subMeshTriangles.Contains(allTriangles[startIndex + i]))
                return true;
        }

        return false;
    }

    private void SearchInAnimator(RaycastHit hitInfo)
    {
        Animator animator = hitInfo.collider.GetComponentInParent<Animator>();

        if (animator == null)
            animator = hitInfo.collider.GetComponentInChildren<Animator>();

        if (animator == null)
            return;

        _materials = _materials.Concat(GetNonParticleRenderer(animator).materials).ToList();
    }

    private Renderer GetNonParticleRenderer(Animator animator)
    {
        Renderer[] allRenderers = animator.GetComponentsInChildren<Renderer>();
        return allRenderers.Where(render => (render as ParticleSystemRenderer) == false).FirstOrDefault();
    }
}