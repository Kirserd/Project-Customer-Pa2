using UnityEngine;
using TMPro;

public class ResultDisplayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _name, _desc, _minuses, _pluses;
    [SerializeField]
    private Transform _point;

    private void Start()
    {
        Vector2 coordinates = PointManager.CalculateCoordinate();
        _point.localPosition = new Vector3(coordinates.x * 100, coordinates.y * 100, 0);
    
        if(coordinates.x > 0 && coordinates.y > 0)
        {
            _name.text = "''Authoritative Parenting''";
            _desc.text = "Balanced and nurturing approach with clear guidelines and open communication. Discipline is supportive, with explanations. " +
                "This parenting style generally leads to positive outcomes for children. Kids raised in authoritative households tend to be confident, responsible, " +
                "and emotionally healthy children who excel academically and have high self-esteem due to encouragement of independence.";
            _minuses.text = "<b>Negative Points:</b>\n - Potential for Over-Parenting\n - Time-Intensive";
            _pluses.text = "<b>Positive Points:</b>\n + Emotional Regulation\n + Independence and Responsibility\n + High Self-Esteem and Academic Achievement";
        }
        else if (coordinates.x <= 0 && coordinates.y > 0)
        {
            _name.text = "''Authoritarian Parenting''";
            _desc.text = "Strict, demanding approach with rigid rules and a focus on obedience. " +
                "Limited flexibility, little explanation for rules, and punishment for mistakes. " +
                "\nChildren tend to behave well but may struggle with aggression, social shyness, and forming relationships. " +
                "Strict rules can lead to rebellion against authority figures as they grow older.";
            _minuses.text = "<b>Negative Points:</b>\n - Lack of Emotional Support\n - Reduced Creativity and Independence \n - Risk of Resentment and Rebellion";
            _pluses.text = "<b>Positive Points:</b>\n + Well-Behaved Behavior\n + Clear Boundaries\n + Respect for Authority";
        }
        else if (coordinates.x <= 0 && coordinates.y <= 0)
        {
            _name.text = "''Neglectful Parenting''";
            _desc.text = "Neglectful parenting, or uninvolved parenting, involves parental detachment and limited involvement in a child's life." +
                " These parents provide basic physical care but lack emotional support, nurturing, and communication." +
                " \nChildren in such families often become self-sufficient and resilient but may struggle with emotions," +
                " forming relationships, and academics due to the absence of guidance and support.";
            _minuses.text = "<b>Negative Points:</b>\n - Emotional Neglect\n - Poor Social Skills and Relationships\n - Academic Challenges";
            _pluses.text = "<b>Positive Points:</b>\n + Self-Sufficiency and Independence\n + Autonomy and Freedom\n + Lower Risk of Parental Pressure";
        }
        else 
        {
            _name.text = "''Permissive Parenting''";
            _desc.text = "Permissive parenting is nurturing and lenient, allowing kids more freedom, which can lead to unhealthy habits like poor eating" +
                " and time management issues. This style may result in impulsiveness, demanding behavior, and limited self-regulation," +
                " affecting social skills and overall development.";
            _minuses.text = "<b>Negative Points:</b>\n - Lack of Structure and Boundaries\n - Impaired Self-Regulation\n - Health Risks";
            _pluses.text = "<b>Positive Points:</b>\n + Independence and Autonomy\n + Open Communication \n + Emotional Support";
        }
    }
}
