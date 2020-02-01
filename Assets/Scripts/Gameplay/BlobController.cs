using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlobController : MonoBehaviour
{

    public GameObject blobPrefab;

    public Transform model;
    
    public BlobController parent;

    public GameObject item;

    public string jumpButton = "Jump_P1";
    public string horizontalCtrl = "Horizontal_P1";
    public string fireButton = "Fire1_P1";

    [SerializeField]
    List<BlobController> _children = new List<BlobController>();

    public List<BlobController> children
    {
        get
        {
            return _children.FindAll(child => child != null);
        }
    }

    [SerializeField]
    int _health = 100;

    public int health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }

    [SerializeField]
    float _size = 1f;

    public float size
    {
        get
        {
            return _size;
        }
        set
        {
            children.ForEach(child => child.size = value);
            _size = value;
            GetComponent<CircleCollider2D>().radius = value / 2f;
        }
    }

    [SerializeField]
    float _distance = 1f;

    public float distance
    {
        get
        {
            return _distance;
        }
        set
        {
            children.ForEach(child => child.distance = value);
            _distance = value;
            GetComponent<CircleCollider2D>().radius = value / 2f;
        }
    }

    [SerializeField]
    float _ballSize = 1f;

    public float ballSize
    {
        get
        {
            return _ballSize;
        }
        set
        {
            children.ForEach(child => child.ballSize = value);
            _ballSize = value;
        }
    }

    [SerializeField]
    float _ballGrowth = 0.5f;

    public float ballGrowth
    {
        get
        {
            return _ballGrowth;
        }
        set
        {
            children.ForEach(child => child.ballGrowth = value);
            _ballGrowth = value;
        }
    }

    [SerializeField]
    float _force = 1f;

    public float force
    {
        get
        {
            return _force;
        }
        set
        {
            children.ForEach(child => child.force = value);
            _force = value;
        }
    }

    [SerializeField]
    float _damping = 0.2f;

    public float damping
    {
        get
        {
            return _damping;
        }
        set
        {
            children.ForEach(child => child.damping = value);
            _damping = value;
        }
    }

    [SerializeField]
    int _frequency = 10;

    public int frequency
    {
        get
        {
            return _frequency;
        }
        set
        {
            children.ForEach(child => child.frequency = value);
            _frequency = value;
        }
    }

    public bool IsFull
    {
        get
        {
            return children.Count > 3;
        }
    }

    public int Count
    {
        get
        {
            return children.Count;
        }
    }

    public bool CanMove
    {
        get
        {
            return Count < 3;
        }
    }
    
    // Total amount of blobs;
    public int TotalCount
    {
        get
        {
            var que = new Queue<BlobController>();
            que.Enqueue(root);
            var count = 0;
            while (que.Count > 0)
            {
                var elem = que.Dequeue();
                count++;
                elem.children.ForEach(child => que.Enqueue(child));
            }
            return count;
        }
    }

    public int Level
    {
        get
        {
            var stack = new Stack<BlobController>();
            stack.Push(this);
            var count = 0;
            while (stack.Count > 0)
            {
                var cur = stack.Pop();
                if (cur.children.Count == 0) break;
                else
                {
                    cur.children.ForEach(child => stack.Push(child));
                    count++;
                }
            }
            return count;
        }
    }

    public BlobController root
    {
        get
        {
            BlobController blob = this;
            while (blob.parent != null)
            {
                blob = blob.parent;
            }
            return blob;
        }
    }

    // Amount of levels in the tree;
    public int TotalLevel
    {
        get
        {
            var stack = new Stack<BlobController>();
            stack.Push(root);
            var count = 0;
            while (stack.Count > 0)
            {
                var cur = stack.Pop();
                if (cur.children.Count == 0) break;
                else
                {
                    cur.children.ForEach(child => stack.Push(child));
                    count++;
                }
            }
            return count;
        }
    }

    public void AddChild(BlobController child)
    {
        _children.Add(child);
    }

    public bool HasSpringTo(BlobController other)
    {
        foreach (var spring in gameObject.GetComponents<SpringJoint2D>())
        {
            if (spring.connectedBody.GetComponent<BlobController>() == this)
            {
                return true;
            }
        }
        return false;
    }

    public void CreateSpring(Rigidbody2D other)
    {
        var spring = gameObject.AddComponent<SpringJoint2D>();
        spring.connectedBody = other;
        spring.frequency = frequency;
        spring.dampingRatio = damping;
        spring.autoConfigureDistance = false;
        spring.distance = distance;
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), other.GetComponent<CircleCollider2D>(), true);
    }

    public void Inherit(BlobController other)
    {
        this.damping = other.damping;
        this.frequency = other.frequency;
        this.size = other.size;
        this.distance = other.distance;
        this.force = other.force;
        this.parent = other;
        this.blobPrefab = other.blobPrefab;
        this.ballGrowth = other.ballGrowth;
        this.ballSize = other.ballSize;

        transform.parent = other.transform;
    }

    public void ParentObject(GameObject obj)
    {
        if (item != null)
        {
            var blobs = new Queue<BlobController>();
            blobs.Enqueue(root);
            while (blobs.Count > 0)
            {
                var cur = blobs.Dequeue();
                if (cur.Count > 0)
                {
                    foreach (var child in children) 
                    {
                        if (child.item == null)
                        {
                            child.ParentObject(item);
                        } else
                        {
                            blobs.Enqueue(child);
                        }
                    }
                }
            }
        } else
        {
            item = obj;
            obj.transform.parent = transform;
            obj.transform.localPosition = Vector3.zero;
        }
    }

    public void CalcJoints()
    {
        foreach (var joint in GetComponentsInChildren<SpringJoint2D>().Union(GetComponents<SpringJoint2D>()))
        {
            DestroyImmediate(joint);
        }
        var blobs = new Queue<BlobController>();
        var childless = new Queue<BlobController>();
        blobs.Enqueue(this);
        while (blobs.Count > 0) {
            var cur = blobs.Dequeue();
            if (cur.Count > 0)
            {
                cur.children.ForEach(child =>
                {
                    cur.CreateSpring(child.GetComponent<Rigidbody2D>());
                    blobs.Enqueue(child);
                });
            } else
            {
                childless.Enqueue(cur);
            }
        }
    }

    public void Bisect()
    {
        if (Count > 0)
        {
            var child = _children.ElementAt(0);
            _children.RemoveAt(0);

            child.transform.parent = null;
            child.parent = null;

            root.CalcJoints();
            child.CalcJoints();

            var force = Random.insideUnitCircle.normalized;
            root.GetComponent<Rigidbody2D>().AddForce(force);
            child.GetComponent<Rigidbody2D>().AddForce(force * new Vector2(-1, -1));

            var blobs = new Queue<BlobController>();
            blobs.Enqueue(child);
            while (blobs.Count > 0)
            {
                var cur = blobs.Dequeue();
                cur.children.ForEach(blob => {
                    var split = blob.gameObject.GetComponent<BlobSplitInteractible>();
                    if (split)
                    {
                        split.enabled = true;
                        split.timer = 0f;
                    }
                    blobs.Enqueue(blob);
                });
            }
        } else
        {
            Kill();
        }
    }

    public void Join(BlobController other)
    {
        foreach (var split in GetComponentsInChildren<BlobSplitInteractible>())
        {
            split.enabled = false;
        }
        GetComponent<BlobSplitInteractible>().enabled = false;
        var blobs = new Stack<BlobController>();
        blobs.Push(other.root);
        while (blobs.Count > 0)
        {
            var cur = blobs.Pop();
            if (!cur.IsFull)
            {
                cur.AddChild(other);
                break;
            }
            else
            {
                cur.children.ForEach(child => blobs.Push(child));
            }
        }
        other.root.CalcJoints();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var interactible = collision.gameObject.GetComponent<IBlobInteractible>();
        if (interactible != null)
        {
            interactible.OnCollideWithBlob(this);
        }
    }

    public void Kill()
    {
        Debug.Log("Die");
        //Destroy(this);
    }

    public void CreateBlob(bool createJoint = true)
    {
        if (this.IsFull)
        {
            int min = children.Min(child => child.Count);
            BlobController least = children.Find(child => child.Count == min);
            least.CreateBlob();
            return;
        }

        var newObj = new GameObject($"{this.gameObject.name}{children.Count}");
        newObj.name = $"{this.gameObject.name}{children.Count}";
        var blob = newObj.AddComponent<BlobController>();
        var collider = newObj.AddComponent<CircleCollider2D>();
        var body = newObj.AddComponent<Rigidbody2D>();
        var split = newObj.AddComponent<BlobSplitInteractible>();
        var newModel = Instantiate(blobPrefab, blob.transform);
        split.enabled = false;
        blob.model = newModel.transform;

        blob.Inherit(this);
        
        newObj.transform.localPosition = Random.insideUnitCircle.normalized;
        body.sharedMaterial = GetComponent<Rigidbody2D>().sharedMaterial;
        AddChild(blob);
        if (createJoint)
        {
            CreateSpring(body);
        }
    }

    public override bool Equals(object other)
    {
        if (other is BlobController)
        {
            return (other as BlobController).gameObject.name == gameObject.name;
        }
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return this.name.GetHashCode();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CircleCollider2D>().radius = size / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove && Mathf.Abs(Input.GetAxis(horizontalCtrl)) > 0.01f)
        {
            GetComponent<Rigidbody2D>().AddTorque(-Input.GetAxis(horizontalCtrl) * force);
        }
        if (item != null) {
            var component = item.GetComponent<IBlobInteractible>();
            if (Input.GetButtonDown(fireButton))
            {
                component.OnAction(this);
            }
            else if (Input.GetButton(fireButton))
            {
                component.OnHoldAction(this);
            }
        }

        if (model != null)
        {
            var val = ballSize + Mathf.Max(Level - 1, 0) * ballGrowth;
            model.localScale = new Vector3(val, val, val);
        }
        if (Input.GetButtonDown(jumpButton))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * 15.0f);
        }
    }
}
