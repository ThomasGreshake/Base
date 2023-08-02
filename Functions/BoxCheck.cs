//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using UnityEngine;

namespace MyFuncs
{
    //Just a simple collision box that can be quickly created and checked

    /* Start and end define a box by defining the box diagonale like so:

                start ----------------------------------------------- x
                -                                                     -
                -                     BOX                             -
                -                                                     -
                x -------------------------------------------------- end

    Order of start and end, as well as which diagonale is used, is irrelevant
    The rotation od the box depends on the coordinate system used!

    Use border to increase the dimensions of box past the start / end dimensions (buffer)
    */

    public struct BoxCheck
    {
        //Data --------------------------------------------------
        private Vector2 position;
        private Vector2 dimensions;


        //Constructor -------------------------------------------------
        public BoxCheck(Vector2 start, Vector2 end, float border = 0)
        {
            dimensions = new Vector2(Mathf.Abs(start.x - end.x) + 2 * border, Mathf.Abs(start.y - end.y) + 2 * border);
            position = new Vector2(Mathf.Min(start.x, end.x) - border, Mathf.Min(start.y, end.y) - border);
        }


        //Publics -----------------------------------------------------
        public bool CheckPosition(Vector2 pos)
        {
            return 
            pos.x >= position.x 
            && pos.x <= position.x + dimensions.x 
            && pos.y >= position.y 
            && pos.y <= position.y + dimensions.y;
        }
    }
}