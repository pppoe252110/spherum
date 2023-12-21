using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene1Controller : MonoBehaviour
{
    [SerializeField] private Transform _cubeRed;
    [SerializeField] private LineRenderer _lineRed;

    [SerializeField] private Transform _cubeGreen;
    [SerializeField] private LineRenderer _lineGreen;
    
    [SerializeField] private MeshRenderer _spheres;

    [SerializeField] private float _moveSpeed = 3f;

    [SerializeField] private Text _distanceText;
    private bool isLoadingNewScene;

    private void Update()
    {
        MoveCubes();

        var poses = new Vector3[] { _cubeRed.position, _cubeGreen.position };
        LinesLogic(poses);

        var dist = Vector3.Distance(poses[0], poses[1]);
        LoadSceneIfDist(dist);

        _lineRed.textureScale = new Vector2(dist * 2f, 1);
        _lineGreen.textureScale = new Vector2(dist * 2f, 1);

        _spheres.enabled = dist < 2f;

        _distanceText.text = string.Format("{0:0.00}", dist);
    }

    private void MoveCubes()
    {
        Vector3 inputRed = Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1);
        Vector3 inputGreen = Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Debug Horizontal"), Input.GetAxisRaw("Debug Vertical")), 1);

        _cubeRed.transform.position += Time.deltaTime * _moveSpeed * inputRed;
        _cubeGreen.transform.position += Time.deltaTime * _moveSpeed * inputGreen;

    }

    private void LinesLogic(Vector3[] poses)
    {
        var matR = _lineRed.material;
        var matG = _lineGreen.material;

        matR.mainTextureOffset = new Vector2(matR.mainTextureOffset.x - Time.deltaTime, matR.mainTextureOffset.y);
        matG.mainTextureOffset = new Vector2(matG.mainTextureOffset.x + Time.deltaTime, matG.mainTextureOffset.y);

        _lineRed.SetPositions(poses);
        _lineGreen.SetPositions(poses);
    }

    private async void LoadSceneIfDist(float dist)
    {
        if (dist < 1f && !isLoadingNewScene)
        {
            isLoadingNewScene = true;
            await SceneManager.LoadSceneAsync("Scene2", LoadSceneMode.Additive);
            await SceneManager.UnloadSceneAsync("Scene1", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            return;
        }
    }
}
