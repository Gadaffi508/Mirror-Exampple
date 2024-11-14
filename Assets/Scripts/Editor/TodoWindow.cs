using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class TodoWindow : EditorWindow
{
    public enum TaskCategory
    {
        Animation,
        Code,
        Test,
        FeedBack
    }

    public enum PriorityLevel
    {
        Hight,
        Middle,
        Low
    }

    private List<TodoItem> todoItems = new List<TodoItem>();
    private string newTaskDescription = "";
    private TaskCategory newTaskCategory = TaskCategory.Animation;
    private TaskCategory filterCategory = TaskCategory.Animation;
    private PriorityLevel newTaskPriority = PriorityLevel.Middle;

    [MenuItem("Window/ToDoWindow")]
    public static void ShowWindow()
    {
        GetWindow<TodoWindow>("ToDo List");
    }

    private void OnGUI()
    {
        if (todoItems == null) todoItems = new List<TodoItem>();

        GUILayout.Label("New Add Mission", EditorStyles.boldLabel);
        newTaskDescription = EditorGUILayout.TextField("Mission Info", newTaskDescription);
        newTaskCategory = (TaskCategory)EditorGUILayout.EnumPopup("Category", newTaskCategory);
        newTaskPriority = (PriorityLevel)EditorGUILayout.EnumPopup("Priority", newTaskPriority);
        filterCategory = (TaskCategory)EditorGUILayout.EnumPopup("Filter by Category", filterCategory);

        if (GUILayout.Button("Add Mission"))
        {
            todoItems.Add(new TodoItem(newTaskDescription, newTaskCategory, newTaskPriority));
            newTaskDescription = "";
            SaveTasks();
        }

        GUILayout.Space(10);
        GUILayout.Label("ToDo List", EditorStyles.boldLabel);

        for (int i = todoItems.Count - 1; i >= 0; i--)
        {
            var item = todoItems[i];
            if (item != null && item.Category == filterCategory)
            {
                GUILayout.BeginHorizontal();
                item.IsCompleted = EditorGUILayout.Toggle(item.IsCompleted, GUILayout.Width(20));
                EditorGUILayout.LabelField(item.Description + " - " + item.Priority.ToString());

                if (GUILayout.Button("Delete", GUILayout.Width(50)))
                {
                    todoItems.RemoveAt(i);
                    SaveTasks();
                    GUILayout.EndHorizontal();
                    continue;
                }

                GUILayout.EndHorizontal();
            }
        }


        GUILayout.Space(50);
        if (GUILayout.Button("Save"))
        {
            SaveTasks();
        }
    }

    [System.Serializable]
    public class TodoItem
    {
        public string Description;
        public bool IsCompleted;
        public TaskCategory Category;
        public PriorityLevel Priority;

        public TodoItem(string description, TaskCategory category, PriorityLevel priority)
        {
            Description = description;
            IsCompleted = false;
            Category = category;
            Priority = priority;
        }
    }


    private void SaveTasks()
    {
        try
        {
            string saveFilePath = Application.persistentDataPath + "/ToDoList.json";
            string json = JsonUtility.ToJson(new TodoListWrapper(todoItems));
            File.WriteAllText(saveFilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving tasks: " + e.Message);
        }
    }

    private void LoadTasks()
    {
        try
        {
            string saveFilePath = Application.persistentDataPath + "/ToDoList.json";
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                TodoListWrapper wrapper = JsonUtility.FromJson<TodoListWrapper>(json);
                todoItems = wrapper.todoItems;
            }
            else
            {
                Debug.LogWarning("No saved tasks found.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading tasks: " + e.Message);
        }
    }

    private void OnEnable()
    {
        LoadTasks();
    }

    private void OnDisable()
    {
        SaveTasks();
    }
    
    [System.Serializable]
    public class TodoListWrapper
    {
        public List<TodoItem> todoItems;

        public TodoListWrapper(List<TodoItem> items)
        {
            todoItems = items;
        }
    }
    
    public class SaveAndLoadjson
    {
        private string saveFilePath;

        public void Save(List<TodoItem> todoItems)
        {
            saveFilePath = Application.persistentDataPath + "/ToDoList.json";
            string json = JsonUtility.ToJson(new TodoListWrapper(todoItems));
            File.WriteAllText(saveFilePath, json);
        }

        public List<TodoItem> Load()
        {
            List<TodoItem> todoItems = new List<TodoItem>();
            if (File.Exists(saveFilePath))
            {
                string loadData = File.ReadAllText(saveFilePath);
                TodoListWrapper wrapper = JsonUtility.FromJson<TodoListWrapper>(loadData);
                todoItems = wrapper.todoItems;
            }
            return todoItems;
        }

        public void DeleteSaveFile()
        {
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }
        }
    }
}