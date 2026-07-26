using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ComputerPlayer : PlayerController
{
    GameObject oasis;

    private Transform currGoal;

    private List<Transform> prevGoals = new List<Transform>();

    private Vector3 prevPos;
    private float unstuckTime = 3f;
    private float stuckTime;

    public float minTurnTime = 5f;
    public float maxTurnTime = 15f;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!isDead && isTurn)
        {
            if (Vector3.Distance(prevPos, transform.position) < 0.25f)
            {
                stuckTime += Time.deltaTime;
                if (stuckTime >= unstuckTime)
                {
                    prevGoals.Add(currGoal);
                    SetGoal();
                }
            }
            else
            {
                stuckTime = 0;
            }
        }

        base.Update();
    }

    public override void Initialize()
    {
        base.Initialize();

        LevelSpawner ls = GameObject.FindObjectOfType<LevelSpawner>();
        oasis = ls.Oasis;
        SetGoal();
    }

    public override void StartTurn()
    {
        base.StartTurn();
        float turnTime = Random.Range(minTurnTime, maxTurnTime);
        Debug.Log($"cpu start turn for {turnTime}s");
        Invoke("EndTurnSelf", turnTime);
    }

    private void EndTurnSelf()
    {
        Debug.Log("cpu end turn");

        if (!isDead && isTurn)
        {
            MeterManager mm = FindObjectOfType<MeterManager>();
            if (mm)
                mm.TogglePlayersTurn();
        }
        
    }

    private void SetGoal()
    {
        Interactable[] interactables = GameObject.FindObjectsOfType<Interactable>();
        float minDist = float.MaxValue;
        GameObject nearestWater = null;

        foreach (var i in interactables)
        {
            if (i.interactionType == Interactable.InteractionType.Water && !prevGoals.Contains(i.transform))
            {
                float d = Vector3.Distance(transform.position, i.transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    nearestWater = i.gameObject;
                }
            }
        }

        if (oasis != null && meter != null)
        {
            float oasisDist = Vector3.Distance(transform.position, oasis.transform.position);
            float timeToOasis = oasisDist / speed + 5f; // add 5 seconds to be safe
            float waterRemaining = meter.PreviewHealth / 3f;
            if (waterRemaining > timeToOasis)
            {
                currGoal = oasis.transform;
                return;
            }
            Debug.Log($"CPU has {waterRemaining} of water. Oasis is {timeToOasis}s away");
        }

        currGoal = nearestWater.transform;
    }

    public override void CollectWater(float amount)
    {
        prevGoals.Add(currGoal);
        base.CollectWater(amount);
        SetGoal();
    }

    protected override Vector2 GetInput()
    {
        if (currGoal == null)
        {
            SetGoal();
            return Vector2.zero;
        }
        Vector3 dir = currGoal.position - transform.position;
        dir.y = 0;
        dir.Normalize();
        return new Vector2(dir.x, dir.z);
    }


}
