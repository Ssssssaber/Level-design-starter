using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Trigger/Attack")]
public class AttackState : State {
    public override void Enter() {
        _machine._agent.isStopped = true;
        //Anim.SetTrigger("attack");
        Debug.Log("enter attack");
    }

    public override void UpdateState() { 
        // Здесь можно добавить поворот к игроку
    }

    public override void OnZoneExit(TriggerZoneType zone, Collider2D other) {
        if (zone == TriggerZoneType.Attack) {
            _machine._agent.isStopped = false;
            _machine.SwitchState(StateID.Chase);
        }
    }

    public override void Exit()
    {
        _machine._agent.isStopped = false;
        Debug.Log("exit attack");
    }
}
