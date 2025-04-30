using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;
    [SerializeField]
    private BulletScriptObject bulletDB;

    void Awake()
    {
        instance = this;
    }
    public BaseSkillCtrl GetBulletByName(string name)
    {
        return bulletDB.SerachByName(name);
    }

    public BaseSkillCtrl GetBulletByID(string id)
    {
        return bulletDB.SerachByID(id);
    }
}
