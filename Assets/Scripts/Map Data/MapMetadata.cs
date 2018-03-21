
/// <summary>
/// Interface for height map metadata containers.
/// Only getters for values that are actually used after data importing are exposed through
/// the interface (during data loading we can use implementations)
/// </summary>

public interface MapMetadata {
    float GetCellsize();
    double GetLowerLeftCornerX();
    double GetLowerLeftCornerY();
    float GetMinHeight();
    float GetMaxHeight();
    MapDataType GetMapDataType();
}
