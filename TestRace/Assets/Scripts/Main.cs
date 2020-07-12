using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Vehicles.Car;

public class Main : MonoBehaviour
{
    private int _carIndex = 0;
    private Camera _currentCamera;
    private bool _changing = false;

    public Camera ChooseCarCamera;
    public GameObject ChooseCarButtons;


    private void Start()
    {
        ChooseCarCamera.depth = 1;
        Time.timeScale = 1;
    }

    private IEnumerator ChangeCar(bool next)
    {
        var _currentCar = transform.GetChild(_carIndex);
        _changing = true;

        _currentCar.GetComponent<Animation>().Play("ExitCar");
        yield return new WaitForSeconds(_currentCar.GetComponent<Animation>().clip.length);
        _currentCar.gameObject.SetActive(false);

        if (next)
        {
            if (_carIndex == transform.childCount - 1)
                _carIndex = -1;
            _carIndex++;
        }
        else
        {
            if (_carIndex == 0)
                _carIndex = transform.childCount - 1;
            _carIndex--;
        }

        _currentCar = transform.GetChild(_carIndex);
        _currentCar.gameObject.SetActive(true);
        yield return new WaitForSeconds(_currentCar.GetComponent<Animation>().clip.length);

        _changing = false;
        StopCoroutine(ChangeCar(next));
    }
    private IEnumerator PlayCar()
    {
        var _currentCar = transform.GetChild(_carIndex);
        _currentCar.GetComponent<Animation>().Play("ExitCar");
        yield return new WaitForSeconds(_currentCar.GetComponent<Animation>().clip.length);
        ChooseCarCamera.depth = -2;
        _currentCar.GetComponentInChildren<CarUserControl>().enabled = true;
        _currentCamera = _currentCar.GetComponentInChildren<Camera>();
        ChooseCarButtons.SetActive(false);
    }

    //Buttons
    public void ChooseCar(bool next)
    {
        if(!_changing)
            StartCoroutine(ChangeCar(next));
    }
    public void Play()
    {
        StartCoroutine(PlayCar());
    }
    public void Continue()
    {
        Time.timeScale = 1;
        _currentCamera.transform.GetChild(0).gameObject.SetActive(false);
        _currentCamera.GetComponent<AudioListener>().enabled = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && !ChooseCarButtons.activeSelf)
        {
            _currentCamera.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
            _currentCamera.GetComponent<AudioListener>().enabled = false;
        }
    }
}
