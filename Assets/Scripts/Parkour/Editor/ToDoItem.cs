
using UnityEditor;
using UnityEngine;

[System.Serializable]
public enum TaskCategory { Animasyon, Kodlama, Test, GeriBildirim }
public class TodoItem
{
    public string Description;
    public bool IsCompleted;
    public TaskCategory Category;

    public TodoItem(string description, TaskCategory category)
    {
        Description = description;
        IsCompleted = false;
        Category = category;
    }
}


