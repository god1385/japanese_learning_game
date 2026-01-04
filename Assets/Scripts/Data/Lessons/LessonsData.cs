using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLesson", menuName = "Game/Lesson", order = 1),]
public class LessonsData : ScriptableObject
{
    public List<SymbolData> lessonSymbols;
}
