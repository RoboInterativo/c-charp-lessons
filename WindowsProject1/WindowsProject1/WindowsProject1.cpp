// WindowsProject1.cpp : Определяет точку входа для приложения.
//

#include "framework.h"
#include "WindowsProject1.h"

#define MAX_LOADSTRING 100

// Глобальные переменные:
HINSTANCE hInst;                                // текущий экземпляр
WCHAR szTitle[MAX_LOADSTRING];                  // Текст строки заголовка
WCHAR szWindowClass[MAX_LOADSTRING];            // имя класса главного окна

// Добавьте после других глобальных переменных
HWND hListBox;
HWND hDetailsEdit;
HWND hAddButton, hDeleteButton, hEditButton, hViewButton, hRefreshButton;

// Отправить объявления функций, включенных в этот модуль кода:
ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    // TODO: Разместите код здесь.

    // Инициализация глобальных строк
    LoadStringW(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadStringW(hInstance, IDC_WINDOWSPROJECT1, szWindowClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance);

    // Выполнить инициализацию приложения:
    if (!InitInstance (hInstance, nCmdShow))
    {
        return FALSE;
    }

    HACCEL hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_WINDOWSPROJECT1));

    MSG msg;

    // Цикл основного сообщения:
    while (GetMessage(&msg, nullptr, 0, 0))
    {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }

    return (int) msg.wParam;
}



//
//  ФУНКЦИЯ: MyRegisterClass()
//
//  ЦЕЛЬ: Регистрирует класс окна.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
    WNDCLASSEXW wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style          = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc    = WndProc;
    wcex.cbClsExtra     = 0;
    wcex.cbWndExtra     = 0;
    wcex.hInstance      = hInstance;
    wcex.hIcon          = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_WINDOWSPROJECT1));
    wcex.hCursor        = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground  = (HBRUSH)(COLOR_WINDOW+1);
    wcex.lpszMenuName   = MAKEINTRESOURCEW(IDC_WINDOWSPROJECT1);
    wcex.lpszClassName  = szWindowClass;
    wcex.hIconSm        = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    return RegisterClassExW(&wcex);
}

//
//   ФУНКЦИЯ: InitInstance(HINSTANCE, int)
//
//   ЦЕЛЬ: Сохраняет маркер экземпляра и создает главное окно
//
//   КОММЕНТАРИИ:
//
//        В этой функции маркер экземпляра сохраняется в глобальной переменной, а также
//        создается и выводится главное окно программы.
//
void RefreshTaskList()
{
    SendMessage(hListBox, LB_RESETCONTENT, 0, 0);

    WIN32_FIND_DATA findFileData;
    HANDLE hFind = FindFirstFile(L"*.txt", &findFileData);

    if (hFind != INVALID_HANDLE_VALUE)
    {
        do
        {
            std::wstring fileName = findFileData.cFileName;
            fileName = fileName.substr(0, fileName.find_last_of(L"."));
            SendMessage(hListBox, LB_ADDSTRING, 0, (LPARAM)fileName.c_str());
        } while (FindNextFile(hFind, &findFileData) != 0);

        FindClose(hFind);
    }
}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
    hInst = hInstance;

    HWND hWnd = CreateWindowW(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, 0, 800, 600, nullptr, nullptr, hInstance, nullptr);

    if (!hWnd)
    {
        return FALSE;
    }

    // Создаем элементы управления
    hListBox = CreateWindowW(L"LISTBOX", L"", WS_CHILD | WS_VISIBLE | WS_BORDER | LBS_NOTIFY | WS_VSCROLL,
        20, 50, 300, 400, hWnd, (HMENU)IDC_TASK_LIST, hInstance, NULL);

    hDetailsEdit = CreateWindowW(L"EDIT", L"", WS_CHILD | WS_VISIBLE | WS_BORDER | ES_MULTILINE | ES_READONLY | WS_VSCROLL,
        350, 50, 400, 400, hWnd, (HMENU)IDC_DETAILS_EDIT, hInstance, NULL);

    hAddButton = CreateWindowW(L"BUTTON", L"Добавить задачу", WS_CHILD | WS_VISIBLE | BS_PUSHBUTTON,
        20, 470, 150, 30, hWnd, (HMENU)IDC_ADD_BUTTON, hInstance, NULL);

    hDeleteButton = CreateWindowW(L"BUTTON", L"Удалить задачу", WS_CHILD | WS_VISIBLE | BS_PUSHBUTTON,
        180, 470, 150, 30, hWnd, (HMENU)IDC_DELETE_BUTTON, hInstance, NULL);

    hViewButton = CreateWindowW(L"BUTTON", L"Просмотреть", WS_CHILD | WS_VISIBLE | BS_PUSHBUTTON,
        350, 470, 150, 30, hWnd, (HMENU)IDC_VIEW_BUTTON, hInstance, NULL);

    hRefreshButton = CreateWindowW(L"BUTTON", L"Обновить список", WS_CHILD | WS_VISIBLE | BS_PUSHBUTTON,
        510, 470, 150, 30, hWnd, (HMENU)IDC_REFRESH_BUTTON, hInstance, NULL);

    // Загружаем список задач
    RefreshTaskList();

    ShowWindow(hWnd, nCmdShow);
    UpdateWindow(hWnd);

    return TRUE;
}

//
//  ФУНКЦИЯ: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  ЦЕЛЬ: Обрабатывает сообщения в главном окне.
//
//  WM_COMMAND  - обработать меню приложения
//  WM_PAINT    - Отрисовка главного окна
//  WM_DESTROY  - отправить сообщение о выходе и вернуться
//
//
//
void ViewSelectedTask()
{
    int selectedIndex = SendMessage(hListBox, LB_GETCURSEL, 0, 0);
    if (selectedIndex != LB_ERR)
    {
        wchar_t taskName[256];
        SendMessage(hListBox, LB_GETTEXT, selectedIndex, (LPARAM)taskName);

        std::wstring filename = taskName + std::wstring(L".txt");
        std::wifstream fin(filename);

        if (fin.is_open())
        {
            std::wstring content((std::istreambuf_iterator<wchar_t>(fin)),
                std::istreambuf_iterator<wchar_t>());
            SetWindowText(hDetailsEdit, content.c_str());
            fin.close();
        }
    }
}

void DeleteSelectedTask()
{
    int selectedIndex = SendMessage(hListBox, LB_GETCURSEL, 0, 0);
    if (selectedIndex != LB_ERR)
    {
        wchar_t taskName[256];
        SendMessage(hListBox, LB_GETTEXT, selectedIndex, (LPARAM)taskName);

        std::wstring filename = taskName + std::wstring(L".txt");
        if (DeleteFile(filename.c_str()))
        {
            RefreshTaskList();
            SetWindowText(hDetailsEdit, L"");
        }
    }
}

INT_PTR CALLBACK AddTaskDialog(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);

    switch (message)
    {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK)
        {
            wchar_t taskName[256], description[1024], status[256];

            GetDlgItemText(hDlg, IDC_TASK_NAME, taskName, 256);
            GetDlgItemText(hDlg, IDC_DESCRIPTION, description, 1024);
            GetDlgItemText(hDlg, IDC_STATUS, status, 256);

            std::wofstream fout(taskName + std::wstring(L".txt"));
            if (fout.is_open())
            {
                fout << L"Название задачи: " << taskName << std::endl;
                fout << L"Описание: " << description << std::endl;
                fout << L"Статус: " << status << std::endl;
                fout << L"Дата: " << GetDateFormat() << std::endl;
                fout.close();
            }

            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        else if (LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
    case WM_COMMAND:
    {
        int wmId = LOWORD(wParam);
        int wmEvent = HIWORD(wParam);

        // Обработка нажатий кнопок
        switch (wmId)
        {
        case IDC_ADD_BUTTON:
            DialogBox(hInst, MAKEINTRESOURCE(IDD_ADD_TASK), hWnd, AddTaskDialog);
            break;

        case IDC_DELETE_BUTTON:
            DeleteSelectedTask();
            break;

        case IDC_VIEW_BUTTON:
            ViewSelectedTask();
            break;

        case IDC_REFRESH_BUTTON:
            RefreshTaskList();
            break;

        case IDM_ABOUT:
            DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
            break;

        case IDM_EXIT:
            DestroyWindow(hWnd);
            break;

        case IDC_TASK_LIST:
            if (wmEvent == LBN_SELCHANGE)
            {
                ViewSelectedTask();
            }
            break;

        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
        }
    }
    break;

    case WM_PAINT:
    {
        PAINTSTRUCT ps;
        HDC hdc = BeginPaint(hWnd, &ps);

        // Рисуем заголовок
        HFONT hFont = CreateFont(24, 0, 0, 0, FW_BOLD, FALSE, FALSE, FALSE,
            DEFAULT_CHARSET, OUT_DEFAULT_PRECIS,
            CLIP_DEFAULT_PRECIS, DEFAULT_QUALITY,
            DEFAULT_PITCH | FF_SWISS, L"Arial");
        SelectObject(hdc, hFont);
        SetTextColor(hdc, RGB(0, 0, 128));
        TextOut(hdc, 20, 20, L"Менеджер задач", 13);
        DeleteObject(hFont);

        EndPaint(hWnd, &ps);
    }
    break;

    case WM_DESTROY:
        PostQuitMessage(0);
        break;

    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}
// Обработчик сообщений для окна "О программе".
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);
    switch (message)
    {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}
