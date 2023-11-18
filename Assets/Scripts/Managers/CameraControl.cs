using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Material shadowedMaterial;

    private Transform mainPlayer;
    private PlayerControl playerControl;
    private Transform mainCamera;
    private Transform mainCamTransformForRaycast;
    private Transform outerCamera;
    

    private bool isUpdate = true;
    private float _timer;
    private LayerMask ignoreMask;
    private Ray ray;
    private RaycastHit hit;
    
    private Dictionary<MeshRenderer, Material> changedMeshRenderers = new Dictionary<MeshRenderer, Material>();
    private HashSet<MeshRenderer> renderers = new HashSet<MeshRenderer>();
    private HashSet<MeshRenderer> changedRanderers = new HashSet<MeshRenderer>();
    private HashSet<MeshRenderer> renderersToReturn = new HashSet<MeshRenderer>();

    private GameManager gm;

    public void SetData(Transform player, Transform _camera, Transform mainCamTransform)
    {
        gm = GameManager.Instance;
        mainCamTransformForRaycast = mainCamTransform;
        mainPlayer = player;
        playerControl = mainPlayer.GetComponent<PlayerControl>();
        mainCamera = _camera;
        outerCamera = mainCamera.parent;
        mainCamera.transform.localPosition = Globals.BasePosition;
        mainCamera.transform.localEulerAngles = Globals.BaseRotation;
        ignoreMask = LayerMask.GetMask(new string[] { "trigger", "player", "ragdoll", "danger" });
    }

    public void SwapControlBody(Transform newTransform)
    {
        mainPlayer = newTransform;
        isUpdate = false;
        StartCoroutine(playSwap());
    }
    private IEnumerator playSwap()
    {
        outerCamera.DOMove(mainPlayer.position/* + basePosition*/, 0.1f);
        yield return new WaitForSeconds(0.1f);
        isUpdate = true;
    }

    public void ResetCameraOnRespawn()
    {
        outerCamera.eulerAngles = Vector3.zero;
    }

    public void ChangeCameraAngleY(float angleY)
    {       
        //outerCamera.eulerAngles = new Vector3(outerCamera.eulerAngles.x, angleY, outerCamera.eulerAngles.z);
    }

    public void ChangeCameraAngleX(float angleX)
    {
        if (!Globals.IsMobile && Mathf.Abs(angleX) > 5) return;
        

        if (angleX > 0 && outerCamera.localEulerAngles.x > 20 && outerCamera.localEulerAngles.x < 30) return;
        if (angleX < 0 && outerCamera.localEulerAngles.x < 340 && outerCamera.localEulerAngles.x > 330) return;

        //if ((angleX > 0 && (outerCamera.localEulerAngles.x < 20 || outerCamera.localEulerAngles.x > 340)) || (angleX < 0 && ((outerCamera.localEulerAngles.x > 340 && outerCamera.localEulerAngles.x < 359.9f) || (outerCamera.localEulerAngles.x >= 0 && outerCamera.localEulerAngles.x < 359.9f))))
        //{
        outerCamera.localEulerAngles = new Vector3(outerCamera.localEulerAngles.x + angleX * 2, outerCamera.localEulerAngles.y, outerCamera.localEulerAngles.z);
        //}
        
    }

    // Update is called once per frame
    void Update()
    {        
        if (!isUpdate || !gm.IsGameStarted) return;
        outerCamera.position = mainPlayer.position/* + basePosition*/;
        
        //if (Globals.IsMobile)
        //{
            outerCamera.eulerAngles = new Vector3(outerCamera.eulerAngles.x, playerControl.angleYForMobile, outerCamera.eulerAngles.z);
        /*}
        else
        {
            outerCamera.eulerAngles = new Vector3(outerCamera.eulerAngles.x, mainPlayer.eulerAngles.y, outerCamera.eulerAngles.z);
        }*/

        if (_timer > 0.1f)
        {
            _timer = 0;
            renderers.Clear();
            renderersToReturn.Clear();
            /*
            if (Physics.Raycast(mainCamTransformForRaycast.position, (mainPlayer.position + Vector3.up - mainCamTransformForRaycast.position).normalized, out hit, 9, ~ignoreMask))
            {
                if (hit.collider.TryGetComponent(out MeshRenderer mr))
                {
                    renderers.Add(mr);

                    if (!changedRanderers.Contains(mr))
                    {
                        changedRanderers.Add(mr);
                        mr.enabled = false;
                    }
                }
            }

            foreach (MeshRenderer item in changedRanderers)
            {
                if (!renderers.Contains(item))
                {
                    renderersToReturn.Add(item);
                }
            }

            if (renderersToReturn.Count > 0)
            {
                foreach (var item in renderersToReturn)
                {
                    if (!item.enabled)
                    {
                        item.enabled = true;
                    }
                    
                    changedRanderers.Remove(item);
                }
            }
            */


            
            if (Physics.Raycast(mainCamTransformForRaycast.position, (mainPlayer.position + Vector3.up - mainCamTransformForRaycast.position).normalized, out hit, 9, ~ignoreMask))
            {
                if (hit.collider.TryGetComponent(out MeshRenderer mr))
                {                    
                    renderers.Add(mr);

                    if (!changedMeshRenderers.ContainsKey(mr))
                    {
                        changedMeshRenderers.Add(mr, mr.material);
                        mr.material = shadowedMaterial;                        
                    }                    
                }
            }

            foreach (MeshRenderer item in changedMeshRenderers.Keys)
            {
                if (!renderers.Contains(item))
                {
                    renderersToReturn.Add(item);                  
                }
            }

            if (renderersToReturn.Count > 0)
            {
                foreach (var item in renderersToReturn)
                {
                    if (item.material != changedMeshRenderers[item])
                    {
                        item.material = changedMeshRenderers[item];
                    }
                    renderers.Remove(item);
                    changedMeshRenderers.Remove(item);
                }
            }

        }
        else
        {
            _timer += Time.deltaTime;
        }
    }
}
