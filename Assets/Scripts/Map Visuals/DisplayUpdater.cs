using Priority_Queue;
using System.Linq;

public class DisplayUpdater {

    private SimplePriorityQueue<UnupdatedDisplay> unupdatedDisplays;

    public DisplayUpdater()
    {
        unupdatedDisplays = new SimplePriorityQueue<UnupdatedDisplay>();
    }

    public void UpdateNextDisplay()
    {
        if (IsEmpty()) return;
        UnupdatedDisplay ud = this.unupdatedDisplays.Dequeue();
        MapDisplay display = ud.display;
        if (display == null) return;
        display.UpdateLOD(ud.lod);
        display.SetStatus(MapDisplayStatus.VISIBLE);
        display.DrawMap();
    }

    public void Clear()
    {
        this.unupdatedDisplays.Clear();
    }

    public void Add(UnupdatedDisplay ud, int lod)
    {
        this.unupdatedDisplays.Enqueue(ud, lod);
    }

    public bool IsEmpty()
    {
        return !this.unupdatedDisplays.Any();
    }
}
