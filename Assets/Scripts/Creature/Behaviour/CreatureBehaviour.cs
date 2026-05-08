using System.Collections;
using UnityEngine;

namespace TricksAndTreatsOrThreats.Behaviour

{
    public class CreatureBehaviour2 : CreatureBehaviour

    {
        void a()
        {
            _moveCoroutine = null;
        }
    }    
    
    public class CreatureBehaviour : DataView<Creature>
    {
        [SerializeField] private Stat speedStat;
        public Animator Animator { get; private set; }

        public Coroutine _moveCoroutine;

        protected override void Subscribe()
        {
            Animator = Instantiate(Data.race.Model, transform);
        }

        protected override void Unsubscribe()
        {
            if (Animator) Destroy(Animator.gameObject);
        }

        public void MoveTo(Vector3 position)
        {
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
            _moveCoroutine = StartCoroutine(Routine(position));
        }

        private IEnumerator Routine(Vector3 position)
        {
            var speed = 1 + Data.stats[speedStat] * 0.2f;
            Animator.SetFloat("speed", speed);

            while (Vector3.Distance(transform.position, position) > 0.01f)
            {
                var direction = (position - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    direction.y = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
                }

                transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
                yield return null;
            }

            transform.position = position;
            Animator.SetFloat("speed", 0);
            _moveCoroutine = null;
        }
    }
}