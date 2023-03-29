using System.Threading.Tasks;
using UnityEngine;
using Views;

public class GameModel 
{
    public int TickTime;

    private bool _onSimulation;

    public void Init()
    {
        TickTime = 1;
    }
    
    
    public async void StartSimulation()
    {
        await Tick(TickTime);
    }
    public async Task Tick(int msec)
    {
        _onSimulation = true;
       
        while (_onSimulation)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5f;
            Vector3 CurMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            if (Input.GetMouseButton(0))
            {
                RaycastHit hitData;
                if (Physics.Raycast(CurMousePos, Vector3.forward*100, out hitData)
                    && hitData.collider.CompareTag("Letter"))
                {
                    WordChecker.Instance.SelectLetterEvent?.Invoke(hitData.transform.gameObject.GetComponent<LetterBlock>());
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                WordChecker.Instance.CheckWordEvent?.Invoke();
            }
            
            await Task.Delay(msec);
        }
            
        EndModel();
    }

    
    public void EndModel()
    {
        _onSimulation = false;
    }
}
