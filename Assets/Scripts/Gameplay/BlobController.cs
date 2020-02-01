using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlobController : MonoBehaviour
{

    public GameObject blobPrefab;

    public Transform model;

    [HideInInspector]
    public BlobController parent;

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
            BlobController root = this;
            while (root.parent != null)
            {
                root = parent;
            }
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

    // Amount of levels in the tree;
    public int TotalLevel
    {
        get
        {
            BlobController root = this;
            while (root.parent != null)
            {
                root = parent;
            }
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

    void CalcJoints()
    {
        var blobs = new Queue<BlobController>();
        var childless = new Queue<BlobController>();
        children.ForEach(blob => blobs?.Enqueue(blob));
        while (blobs.Count > 0)
        {
            var blob = blobs.Dequeue();
            if (blob.Count == 0)
            {
                childless.Enqueue(blob);
            }
            blob.children.ForEach(other => blobs?.Enqueue(other));
        }

        //childless.ToList<BlobController>().ForEach(blob => Debug.Log(blob));
    }

    public void CreateBlob()
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
        var newModel = Instantiate(blobPrefab, blob.transform);
        blob.model = newModel.transform;

        blob.Inherit(this);
        
        newObj.transform.localPosition = Random.insideUnitCircle.normalized;
        body.sharedMaterial = GetComponent<Rigidbody2D>().sharedMaterial;
        _children.Add(blob);
        CreateSpring(body);


        CalcJoints();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f)
        {
            GetComponent<Rigidbody2D>().AddTorque(Input.GetAxis("Horizontal") * force);
        }
        if (model != null)
        {
            var val = ballSize + Mathf.Max(Level - 1, 0) * ballGrowth;
            model.localScale = new Vector3(val, val, val);
        }
    }
}
