using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IShip
{
    bool IsDead();
    void TakeDamage(float value);
}

