using UnityEngine;

public class DisplayNeighborRelation : MapNeighborRelation { 
    
    private DisplayReadySlice firstDRSlice, secondDRSlice;
    private MapDisplay firstDisplay, secondDisplay;
    public DisplayNeighborRelation(MapData firstMember, MapData secondMember, NeighborType neighborType) : 
    base(firstMember, secondMember, neighborType){}

    public void AddDisplayReadySlice(DisplayReadySlice member) {
        if(member.baseSlice == firstMember) firstDRSlice = member;
        else if (member.baseSlice == secondMember) secondDRSlice = member;
        else throw new System.ArgumentException("Tried to add display ready slice w/ non-existing baseSlice!");
    }

    public void AddDisplay(DisplayReadySlice member, MapDisplay display) {
        if(member.baseSlice == firstMember) firstDisplay = display;
        else if (member.baseSlice == secondMember) secondDisplay = display;
        else throw new System.ArgumentException("Tried to add display to a non-existing member!");
    }

    public MapDisplay GetOtherDisplay(DisplayReadySlice member) {
        if(member.baseSlice == firstMember) return secondDisplay;
        else if (member.baseSlice == secondMember) return firstDisplay;
        else throw new System.ArgumentException("Tried to GetOtherDisplay for non-existing member!");
    }

    private Mesh GetMesh(MapDisplay display) {
        if(display != null) {
            MeshFilter filter = display.meshFilter;
            if(filter != null) return filter.sharedMesh;
        }
        return null;
    }

    public Mesh GetOtherMesh(DisplayReadySlice member) {
        return GetMesh(GetOtherDisplay(member));
    }

    public DisplayReadySlice GetOtherDRSlice(DisplayReadySlice member) {
        if(member.baseSlice == firstMember) return secondDRSlice;
        else if (member.baseSlice == secondMember) return firstDRSlice;
        else throw new System.ArgumentException("Tried to GetOtherDRSlice for non-existing member!");
    }

    public int GetOtherWidth(DisplayReadySlice member) {
        return base.GetOtherWidth(member.baseSlice);
    }

    public int GetOtherHeight(DisplayReadySlice member) {
        return base.GetOtherHeight(member.baseSlice);
    }

    public bool IsFirstMember(DisplayReadySlice member) {
        return base.IsFirstMember(member.baseSlice);
    }
}
