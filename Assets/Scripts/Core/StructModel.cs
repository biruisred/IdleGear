using System;

namespace IdleGear
{
    [Serializable]
    public struct Place
    {
        public string id;
        public ExplorePlace[] explorePlaces;
    }

    [Serializable]
    public struct ExplorePlace
    {
        public string id;
    }
    
    [Serializable]
    public struct Monster
    {
        public string id;
        public int hp;
    }
}