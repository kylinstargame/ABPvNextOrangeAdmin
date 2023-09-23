using System.Collections.Generic;

namespace ABPvNextOrangeAdmin.Common;

public interface ITreeSelectNode<T>
{
    public long Id { get; set; }
    
    public string Label { get; set; }
    
    public List<T> Children { get; set; }
}