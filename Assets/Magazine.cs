using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Interactive
{
    public GameObject mOtherMagazine;

    private Interaction mThisInteraction = null;
    private MeshRenderer mOtherMagazineRenderer = null;

    private Coroutine mMeasureCheck = null;
    private bool isLoaded = false;

    void Awake()
    {
        mOtherMagazineRenderer = mOtherMagazine.GetComponent<MeshRenderer>();
    }

    public override void OnPickup(Interaction newInteraction)
    {
        mOtherMagazineRenderer.enabled = true;
        mThisInteraction = newInteraction;
    }

    public override void OnDrop(Interaction newInteraction)
    {
        mOtherMagazineRenderer.enabled = false;
        mThisInteraction = null;

        if (mMeasureCheck != null)
        {
            StopCoroutine(mMeasureCheck));
            
            transform.position = mOtherMagazine.transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() !== mOtherMagazine.GetInstanceID())
        {
            return;
        }
        RemoveFromController();
    }

    private void RemoveFromController()
    {
        mOtherMagazineRenderer?.enabled = false;

        mThisInteraction.Detach();

        GetComponent<Collider>().enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        transform.parent = mOtherMagazine.transform;
        transform.localRotation = Quaternion.identity;

        float interactDistance = (mThisInteraction.gameObject.transform.position - mOtherMagazine.transform.position).sqrMagnitude;
        mMeasureCheck = StartCoroutine(MeasureInteract(interactDistance));
    }

    private IEnumerator MeasureInteract(float interactDistance)
    {
        while (interactDistance * 4 > (mThisInteraction.gameObject.transform.position - mOtherMagazine.transform.position).sqrMagnitude)
        {
            float step = 0.2f * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, mOtherMagazine.transform.position, step);
            yield return null;
        }

        ReturnToController();
    }

    private void ReturnToController()
    {
       mOtherMagazineRenderer.enabled = true;

        transform.parent = null;
        transform.position = mThisInteraction.gameObject.transform.position;
        
        GetComponent<Collider>().enabled = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        mThisInteraction.Attach();
    }
}
