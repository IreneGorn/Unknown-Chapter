using UnityEngine;

namespace UI.InventoryScripts
{
    [System.Serializable]
    public class Item
    {
        public int id;
        public string name;
        public int count;
        public Sprite img;
        // добавить описание, bool значение одет предмет или нет, максимум два кольца и одно ожерелье
    }
}
