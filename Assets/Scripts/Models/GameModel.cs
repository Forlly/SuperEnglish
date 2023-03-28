using System.Threading.Tasks;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    public int TickTime;

    private bool _onSimulation;

    public void Init()
    {
        TickTime = 10;
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
          
            
            await Task.Delay(msec);
        }
            
        EndModel();
    }

    
    public void EndModel()
    {
        _onSimulation = false;
    }
}
