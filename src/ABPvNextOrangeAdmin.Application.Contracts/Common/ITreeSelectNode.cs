using System.Collections.Generic;

namespace ABPvNextOrangeAdmin.Common;

public interface ITreeSelectNode<T>
{
    public string Label { get; set; }
    
    public List<T> Children { get; set; }
}