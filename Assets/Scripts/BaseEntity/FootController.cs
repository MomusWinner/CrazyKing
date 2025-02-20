using System;
using UnityEngine;

namespace BaseEntity
{
    public class FootController : MonoBehaviour
    {
        private class Foot
        {
            public bool BindFootToParent { get; set; }
            public bool Block { get; set; }
            public bool IsStepping { get; private set; }
            public float Acceleration { get; set; } = 25f;
            public float StartSpeed { get; set; } = 2f;
            public float StepRadius { get; set; } = 0.4f;
            public float MaxSpeed { get; set; } = 20f;
            public float MaxFootSize { get; set; } = 1.3f;
            
            private readonly GameObject _foot;
            private float _currentSpeed;
            private Vector3 _footPos;
            private Vector3 _startStepPosition;
            private Vector3 _endStepPosition;
            
            private SpriteRenderer _spriteRenderer;
            
            public Foot(GameObject foot, SpriteRenderer spriteRenderer)
            {
                _spriteRenderer = spriteRenderer;
                _foot = foot;
                _footPos = foot.transform.position;
                _currentSpeed = StartSpeed;
            }

            public void Update(float dt, Vector3 startPos)
            {
                if (!Block)
                {
                    if (!IsStepping)
                    {
                        float stepRadius = (_footPos - startPos).sqrMagnitude;
                        if (stepRadius > (StepRadius * StepRadius))
                        {
                            BindFootToParent = false;
                            IsStepping = true;
                            _startStepPosition = _footPos;
                        }
                    }
                    
                    if (IsStepping)
                    {
                        _endStepPosition = (startPos - _startStepPosition).normalized * StepRadius + startPos;
                        
                        float distanceToTarget = (_endStepPosition - _footPos).sqrMagnitude;
                        if (distanceToTarget > 0.01f)
                        {
                            // position
                            Vector3 dir = _endStepPosition - _footPos;
                            dir.Normalize();
                            if (_currentSpeed < MaxSpeed)
                                _currentSpeed += Acceleration * dt;
                            _footPos += dir * (_currentSpeed * dt);
                            
                            //size
                            float footSize = FootSize(_endStepPosition);
                            _foot.transform.localScale = new Vector3(footSize, footSize, 1);
                        }
                        else
                        {
                            IsStepping = false;
                            _currentSpeed = StartSpeed;
                        }
                    }
                }

                if (BindFootToParent)
                    _footPos = _foot.transform.position;
                else
                    _foot.transform.position = _footPos;
            }

            public void Show()
            {
                _spriteRenderer.enabled = true;
            }

            public void Hide()
            {
                _spriteRenderer.enabled = false;
            }
            
            private float FootSize(Vector3 endPos)
            {
                float targetStepLength = (endPos - _startStepPosition).magnitude;
                float stepLength = (_footPos - _startStepPosition).magnitude;
                
                float c = stepLength / targetStepLength;
                float footSize = -4 * c * c + c * 4; // from 0 to 1
                footSize = 1f + footSize * (MaxFootSize - 1f); // from 1 to MaxFootSize
                return footSize;
            }
        }
        
        [SerializeField] private GameObject _footLeft;
        [SerializeField] private GameObject _footRight;

        [SerializeField] private float _width;
        
        [SerializeField] private float _stepRadius;
        [SerializeField] private float _acceleration = 1f;
        [SerializeField] private float _startSpeed = 0.2f;
        [SerializeField] private float _maxSpeed = 11f;
        [SerializeField] private float _maxFootSize = 1.3f;

        private Vector3 FootLeftStartPos => transform.position + transform.up * _width;
        private Vector3 FootRightStartPos => transform.position - transform.up * _width;

        private Foot _leftFoot;
        private Foot _rightFoot;
        
        public void Start()
        {
            _footLeft.transform.position = FootLeftStartPos;
            _footRight.transform.position = FootRightStartPos;
            
            SpriteRenderer leftFootSprite = _footLeft.GetComponentInChildren<SpriteRenderer>() ?? throw new ArgumentNullException("_footLeft.GetComponentInChildren<SpriteRenderer>()");
            SpriteRenderer rightFootSprite = _footRight.GetComponentInChildren<SpriteRenderer>();
            
            _rightFoot = new Foot(_footRight, rightFootSprite)
            {
                Acceleration = _acceleration,
                StartSpeed = _startSpeed,
                StepRadius = _stepRadius,
                MaxSpeed = _maxSpeed,
                MaxFootSize = _maxFootSize,
            };

            _leftFoot = new Foot(_footLeft, leftFootSprite)
            {
                Acceleration = _acceleration,
                StartSpeed = _startSpeed,
                StepRadius = _stepRadius,
                MaxSpeed = _maxSpeed,
                MaxFootSize = _maxFootSize,
            };
        }

        public void Update()
        {
#if UNITY_EDITOR 
            _rightFoot.StartSpeed = _startSpeed;
            _rightFoot.Acceleration = _acceleration;
            _rightFoot.MaxSpeed = _maxSpeed;
            _rightFoot.StepRadius = _stepRadius;
            _rightFoot.MaxFootSize = _maxFootSize;
            
            _leftFoot.StartSpeed = _startSpeed;
            _leftFoot.Acceleration = _acceleration;
            _leftFoot.MaxSpeed = _maxSpeed;
            _leftFoot.StepRadius = _stepRadius;
            _leftFoot.MaxFootSize = _maxFootSize;
#endif
            // right foot
            _rightFoot.Block = _leftFoot.IsStepping;
            bool rightFootInDeadZone = Vector3.Distance(_footRight.transform.position, FootRightStartPos) > 1.3f * _stepRadius;
            if (_rightFoot.Block && rightFootInDeadZone)
                _rightFoot.BindFootToParent = true;
            _rightFoot.Update(Time.deltaTime, FootRightStartPos);
            
            // left foot
            bool leftFootInDeadZone = Vector3.Distance(_footLeft.transform.position, FootLeftStartPos) > 1.3f * _stepRadius;
            _leftFoot.Block = _rightFoot.IsStepping;
            if (_leftFoot.Block && leftFootInDeadZone)
                _leftFoot.BindFootToParent = true;
            _leftFoot.Update(Time.deltaTime, FootLeftStartPos);
        }

        public void HideLeftFoot()
        {
            _leftFoot.Hide();
        }

        public void HideRightFoot()
        {
            _rightFoot.Hide();
        }

        public void ShowLeftFoot()
        {
            _leftFoot.Show();
        }

        public void ShowRightFoot()
        {
            _rightFoot.Show();
        }
        
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Gizmos.DrawSphere(FootLeftStartPos, 0.05f);
            Gizmos.DrawWireSphere(FootLeftStartPos, _stepRadius);
            
            Gizmos.DrawSphere(FootRightStartPos, 0.05f);
            Gizmos.DrawWireSphere(FootRightStartPos, _stepRadius);
        }
    }
}