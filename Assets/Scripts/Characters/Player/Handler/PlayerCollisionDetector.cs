using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    [SerializeField] private string[] _dangerTags;

    private void OnTriggerEnter(Collider other)
    {
        if (_dangerTags != null && _dangerTags.Length > 0)
        {
            for (int i = 0; i < _dangerTags.Length; i++)
            {
                if (other.tag == _dangerTags[i])
                {
                    Death();
                }
            }
        }
    }

    private void Death()
    {
        GameManager.RestartScene();
    }
}
