using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{

  public event EventHandler<OnProgressEventArgs> OnProgressChanged;
  public class OnProgressEventArgs : EventArgs
  {
    public float progressNormalized;
  }

}
