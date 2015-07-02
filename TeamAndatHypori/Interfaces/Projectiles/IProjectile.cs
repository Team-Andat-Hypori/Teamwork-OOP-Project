using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamAndatHypori.Enums;

namespace TeamAndatHypori.Interfaces.Projectiles
{
    public interface IProjectile
    {
        int Speed { get;}
        Direction Direction { get; }
        int Damage { get; }
    }
}
