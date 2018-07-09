using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractButtonOpenPanel : MonoBehaviour
{
    public enum SpawnChild { FirstPoolItem , SecendPoolItem }

    private Dictionary<Button, GameObject> map = new Dictionary<Button, GameObject>();
    protected Dictionary<Button, List<GameObject>> listmap = new Dictionary<Button, List<GameObject>>();
    protected Button lastkey;

    //多缓存池
    protected SimpleObjectPool<GameObject> selfMainpool;
    protected SimpleObjectPool<GameObject> selfSecendpool;
    //主缓存池长度
    protected int mainpoollength;
    //辅缓存池长度
    protected int secendpoollength;
    //记住生成的物体用于回收
    private Dictionary<SpawnChild,List<GameObject>> Spawnall = new Dictionary<SpawnChild, List<GameObject>>();


    protected void RegestToMap(Button b,GameObject go)
    {
        map.Add(b,go);
    }

    /// <summary>
    /// 生成预设物体
    /// </summary>
    /// <param name="prefab">预设物体</param>
    /// <returns></returns>
    protected GameObject SpawnChildren(SpawnChild sc,Transform parent)
    {
        GameObject go = null;

        if (!Spawnall.ContainsKey(sc))
        {
            Spawnall.Add(sc, new List<GameObject>());
        }

        switch (sc)
        {
            case SpawnChild.FirstPoolItem:
                go = selfMainpool.Allocate();
                break;
            case SpawnChild.SecendPoolItem:
                go = selfSecendpool.Allocate();//Instantiate(prefab);
                break;
            default:
                break;
        }
        go.SetActive(true);
        go.transform.SetParent(parent);
        Spawnall[sc].Add(go);
        return go;
    }

    protected void InitPool(int size1,GameObject prefab1,int size2 = 0,GameObject prefab2 = null)
    {
        mainpoollength = size1;
        secendpoollength = size2;
        selfMainpool = new SimpleObjectPool<GameObject> (()=>  Instantiate(prefab1),null,size1);
        if (size2 == 0)
        {
            return;
        }
        selfSecendpool = new SimpleObjectPool<GameObject>(() => Instantiate(prefab2), null, size2);
    }


    protected void UnspawnAll()
    {
        foreach (KeyValuePair<SpawnChild,List<GameObject>> item in Spawnall)
        {
            List<GameObject> golist = item.Value;
            for (int i = 0; i < golist.Count; i++)
            {
                GameObject g = golist[i];
                g.GetComponent<Image>().color = Color.white;
                switch (item.Key)
                {
                    case SpawnChild.FirstPoolItem:
                        selfMainpool.Recycle(g);
                        break;
                    case SpawnChild.SecendPoolItem:
                        selfSecendpool.Recycle(g);
                        break;
                    default:
                        break;
                }
            }
        }
        Spawnall.Clear();
    }


    /// <summary>
    /// 根据点击的button选择对应的panel
    /// </summary>
    /// <param name="bu">点击button</param>
    protected virtual void ChoisePanel(Button bu)
    {
        if (bu == lastkey)
        {
            return;
        }
        if (null != lastkey)
        {
            if (map.ContainsKey(lastkey))
            {
                lastkey.GetComponent<Image>().color = Color.white;
                IPanelItem ipi = map[lastkey].gameObject.GetComponent<IPanelItem>();
                if (null != ipi)
                {
                    ipi.OnLeveThisPage();
                }
                map[lastkey].gameObject.SetActive(false);
            }
            if (listmap.ContainsKey(lastkey))
            {
                for (int i = 0; i < listmap[lastkey].Count; i++)
                {
                    listmap[lastkey][i].SetActive(false);
                }
            }
        }
        if (map.ContainsKey(bu))
        {
            bu.GetComponent<Image>().color = Color.green;
            map[bu].gameObject.SetActive(true);
            IPanelItem ipi = map[bu].gameObject.GetComponent<IPanelItem>();
            if (null != ipi)
            {
                ipi.OnEnterThisPage();
            }

        }
        if (listmap.ContainsKey(bu))
        {
            for (int i = 0; i < listmap[bu].Count; i++)
            {
                listmap[bu][i].SetActive(true);
            }
        }
        lastkey = bu;
        ChoiseEnd(bu);
    }

    protected virtual void ChoiseEnd(Button bu) { }

}
