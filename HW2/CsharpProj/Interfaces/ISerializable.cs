﻿using System;
namespace CsharpProj
{
    public interface ISerializable
    {
        public void FromWord(String word);

        public String ToWord();
    }
}