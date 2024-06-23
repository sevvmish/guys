using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Material shadowedMaterial;

    private Transform mainPlayer;
    private PlayerControl playerControl;
    private Transform mainCamera;
    private Transform mainCamTransformForRaycast;
    private Transform outerCamera;
    private Vector3 outerCameraShiftVector = Vector3.zero;


    private float currentZoom;
    private float zoomTimer;

    private bool isUpdate = true;
    private float _timer;
    private LayerMask ignoreMask;
    private Ray ray;
    private RaycastHit hit;

    private float xLimitUp = 40;
    private float xLimitDown = 320;

    private bool isPhotoRegime = false;
    private Transform photoPoint;
    private GameObject mainCanvas;

    private Dictionary<MeshRenderer, Material> changedMeshRenderers = new Dictionary<MeshRenderer, Material>();
    private HashSet<MeshRenderer> renderers = new HashSet<MeshRenderer>();
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
        mainCamera.localPosition = Globals.BasePosition;
        mainCamera.localEulerAngles = Globals.BaseRotation;

        //photoPoint = GameObject.Find("PhotoPoint").transform;
        //mainCanvas = GameObject.Find("Gameplay Canvas"); 

        switch(gm.GetLevelManager().GetCurrentLevelType())
        {
            case LevelTypes.level5:
                xLimitDown += 29;
                outerCamera.eulerAngles += new Vector3(10, 0, 0);
                outerCameraShiftVector = new Vector3(10, 0, 0);
                break;

            case LevelTypes.level3:                
                outerCamera.eulerAngles += new Vector3(15, 0, 0);
                outerCameraShiftVector = new Vector3(15, 0, 0);
                break;

            case LevelTypes.level6:
                outerCamera.eulerAngles += new Vector3(15, 0, 0);
                outerCameraShiftVector = new Vector3(15, 0, 0);
                break;

            case LevelTypes.level16:
                //mainCamera.localEulerAngles = new Vector3(70, 0, 0);
                outerCamera.eulerAngles += new Vector3(-10, 0, 0);
                outerCameraShiftVector = new Vector3(-10, 0, 0);
                break;

            default:
                
                //outerCamera.eulerAngles += new Vector3(5, 0, 0);
                //outerCameraShiftVector = new Vector3(5, 0, 0);
                break;
        }

        ignoreMask = LayerMask.GetMask(new string[] { "trigger", "player", "ragdoll", "danger" });

        currentZoom = Globals.MainPlayerData.Zoom;
        Zoom(Globals.MainPlayerData.Zoom);
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

    public void ChangeZoom(float koeff)
    {                
        if (koeff > 0 && Globals.MainPlayerData.Zoom < Globals.ZOOM_LIMIT)
        {
            float add = Globals.ZOOM_DELTA;
            mainCamera.position += mainCamera.forward * add;
            Globals.MainPlayerData.Zoom += add;
                        
        }
        else if(koeff < 0 && Globals.MainPlayerData.Zoom > -Globals.ZOOM_LIMIT)
        {
            float add = -Globals.ZOOM_DELTA;
            mainCamera.position += mainCamera.forward * add;
            Globals.MainPlayerData.Zoom += add;
        }

        checkCorrectZoom();
    }

    private void checkCorrectZoom()
    {
        if (Globals.MainPlayerData.Zoom > Globals.ZOOM_LIMIT)
        {
            Globals.MainPlayerData.Zoom = Globals.ZOOM_LIMIT;
            Zoom(Globals.MainPlayerData.Zoom);
        }
        else if (Globals.MainPlayerData.Zoom < -Globals.ZOOM_LIMIT)
        {
            Globals.MainPlayerData.Zoom = -Globals.ZOOM_LIMIT;
            Zoom(Globals.MainPlayerData.Zoom);
        }
    }

    public void Zoom(float koeff)
    {
        mainCamera.localPosition = Globals.BasePosition;
        mainCamera.position += mainCamera.forward * koeff;
    }

    public void RotateCameraOnVector(Vector3 vec, float _time)
    {
        outerCamera.DORotate(outerCamera.eulerAngles + vec, _time).SetEase(Ease.Linear);
        //outerCameraShiftVector += vec;
    }

    public void ResetCameraOnRespawn()
    {
        outerCamera.eulerAngles = Vector3.zero;
    }

    public void ResetCameraOnRespawn(Vector3 vec)
    {
        outerCamera.eulerAngles = new Vector3(outerCameraShiftVector.x, vec.y, 0);
    }

    public void ChangeCameraAngleY(float angleY)
    {
        
        outerCamera.eulerAngles = new Vector3(outerCamera.eulerAngles.x, playerControl.angleYForMobile + angleY, outerCamera.eulerAngles.z);
    }

    public void ChangeCameraAngleX(float angleX)
    {
        if (!Globals.IsMobile && Mathf.Abs(angleX) > 5) return;
        

        if (angleX > 0 && outerCamera.localEulerAngles.x > (xLimitUp - 10) && outerCamera.localEulerAngles.x < xLimitUp) return;
        if (angleX < 0 && outerCamera.localEulerAngles.x < (xLimitDown + 10) && outerCamera.localEulerAngles.x > xLimitDown) return;

        //if ((angleX > 0 && (outerCamera.localEulerAngles.x < 20 || outerCamera.localEulerAngles.x > 340)) || (angleX < 0 && ((outerCamera.localEulerAngles.x > 340 && outerCamera.localEulerAngles.x < 359.9f) || (outerCamera.localEulerAngles.x >= 0 && outerCamera.localEulerAngles.x < 359.9f))))
        //{
        outerCamera.localEulerAngles = new Vector3(outerCamera.localEulerAngles.x + angleX * 2, outerCamera.localEulerAngles.y, outerCamera.localEulerAngles.z);
        //}
        
    }

    // Update is called once per frame
    void Update()
    {        
        if (zoomTimer > 2)
        {
            zoomTimer = 0;

            if (currentZoom != Globals.MainPlayerData.Zoom)
            {                
                currentZoom = Globals.MainPlayerData.Zoom;
                SaveLoadManager.Save();
            }
        }
        else
        {
            zoomTimer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            isPhotoRegime = !isPhotoRegime;
        }

        if (isPhotoRegime && photoPoint != null)
        {
            outerCamera.position = photoPoint.position;
            outerCamera.eulerAngles = photoPoint.eulerAngles;
            outerCamera.transform.GetChild(0).localPosition = Vector3.zero;
            outerCamera.transform.GetChild(0).localEulerAngles = Vector3.zero;
            mainCanvas.SetActive(false);
        }
        else
        {
            outerCamera.position = mainPlayer.position;
            outerCamera.eulerAngles = new Vector3(outerCamera.eulerAngles.x, playerControl.angleYForMobile, outerCamera.eulerAngles.z);
        }
        

        if (!isUpdate || !gm.IsGameStarted) return;

        if (_timer > 0.1f)
        {
            _timer = 0;
            renderers.Clear();
            renderersToReturn.Clear();

            float distance = (mainPlayer.position + Vector3.up - mainCamTransformForRaycast.position).magnitude;

            if (Physics.Raycast(mainCamTransformForRaycast.position, (mainPlayer.position + Vector3.up - mainCamTransformForRaycast.position).normalized, out hit, distance, ~ignoreMask))
            {
                if (hit.collider.TryGetComponent(out MeshRenderer mr) && !hit.collider.gameObject.isStatic)
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
