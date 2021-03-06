﻿using Priority_Queue;
using System.Linq;

/// <summary>
/// Handles the re-drawing of MapDisplays.
/// </summary>
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
        DoUpdate(display, ud.lod);
    }

    public virtual void DoUpdate(MapDisplay display, int lod) {
        display.SetStatus(MapDisplayStatus.VISIBLE);
        display.UpdateLOD(lod);
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
