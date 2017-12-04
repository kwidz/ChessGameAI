using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Coups
{
    public String positionDepart;
    public String positionArrivee;
    int Value;
    public Coups(String positionDepart, String positionArrivee, int Value)
    {
        this.Value = Value;
        this.positionArrivee = positionArrivee;
        this.positionDepart = positionDepart;
    }

}