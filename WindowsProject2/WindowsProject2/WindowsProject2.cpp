#include "framework.h"
#include "WindowsProject2.h"
#include <string>
#include <fstream>

#define MAX_LOADSTRING 100
#define IDC_NAME_EDIT 101
#define IDC_DESCRIPTION_EDIT 102
#define IDC_STATUS_EDIT 103
#define IDC_DATE_EDIT 104
#define IDC_ADD_BUTTON 105
#define IDC_DELETE_BUTTON 106
#define IDC_VIEW_BUTTON 107
#define IDC_LISTBOX 108
#define IDM_ABOUT 40001
#define IDM_EXIT 40002
#define IDD_ABOUTBOX 103
#define IDC_TASKMANAGERGUICPP 100
#define IDI_TASKMANAGERGUICPP 101
#define IDI_SMALL 102
#define IDS_APP_TITLE 103

// Глобальные переменные
HINSTANCE hInst;
WCHAR szTitle[MAX_LOADSTRING];
WCHAR szWindowClass[MAX_LOADSTRING];
HWND hNameEdit, hDescriptionEdit, hStatusEdit, hDateEdit, hListBox;

// Класс Tasc_Act1
class Tasc_Act1 {
public:
    std::string name;
    std::string opis;
    std::string status;
    double data;

    void aadTask(const std::string& name, const std::string& opis, const std::string& status, double data) {
        this->name = name;
        this->opis = opis;
        this->status = status;
        this->data = data;
        std::ofstream fout(name + ".txt");
        if (fout.is_open()) {
            fout << "Название задачи: " << name << std::endl;
            fout << "Описание задачи: " << opis << std::endl;
            fout << "Статус задачи: " << status << std::endl;
            fout << "Дата до окончания задачи: " << data << std::endl;
            MessageBoxA(NULL, "Задача успешно добавлена!", "Успех", MB_OK);
        }
        fout.close();
    }

    void delateTask(const std::string& name) {
        if (remove((name + ".txt").c_str()) == 0) {
            MessageBoxA(NULL, "Задача успешно удалена!", "Успех", MB_OK);
        }
        else {
            MessageBoxA(NULL, "Ошибка: задача не найдена!", "Ошибка", MB_OK);
        }
    }

    void printTask(const std::string& name) {
        std::string line, result;
        std::ifstream fin(name + ".txt");
        if (!fin.is_open()) {
            MessageBoxA(NULL, "Ошибка: не удалось открыть задачу!", "Ошибка", MB_OK);
            return;
        }
        while (std::getline(fin, line)) {
            result += line + "\n";
        }
        fin.close();
        MessageBoxA(NULL, result.c_str(), "Задача", MB_OK);
    }

    void printVtask(HWND hListBox) {
        SendMessage(hListBox, LB_RESETCONTENT, 0, 0); // Очистить список

        WIN32_FIND_DATAW ffd;
        HANDLE hFind = FindFirstFileW(L"*.txt", &ffd);

        if (hFind != INVALID_HANDLE_VALUE) {
            do {
                SendMessage(hListBox, LB_ADDSTRING, 0, (LPARAM)ffd.cFileName);
            } while (FindNextFileW(hFind, &ffd) != 0);
            FindClose(hFind);
        }
    }
};

Tasc_Act1 task;

// Прототипы функций
ATOM MyRegisterClass(HINSTANCE hInstance);
BOOL InitInstance(HINSTANCE, int);
LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK About(HWND, UINT, WPARAM, LPARAM);

int APIENTRY wWinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPWSTR lpCmdLine, _In_ int nCmdShow) {
    WNDCLASSEXW wcex = { sizeof(WNDCLASSEX) };
    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = WndProc;
    wcex.hInstance = hInstance;
    wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wcex.lpszClassName = L"TestWindowClass";

    if (!RegisterClassExW(&wcex)) {
        MessageBoxW(NULL, L"Failed to register class!", L"Error", MB_OK | MB_ICONERROR);
        return 1;
    }

    HWND hWnd = CreateWindowW(L"TestWindowClass", L"Test Window", WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, 0, 600, 400, nullptr, nullptr, hInstance, nullptr);

    if (!hWnd) {
        MessageBoxW(NULL, L"Failed to create window!", L"Error", MB_OK | MB_ICONERROR);
        return 1;
    }

    ShowWindow(hWnd, SW_SHOW);
    UpdateWindow(hWnd);

    MSG msg;
    while (GetMessage(&msg, nullptr, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return (int)msg.wParam;
}

//LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) {
//    switch (message) {
//    case WM_DESTROY:
//        PostQuitMessage(0);
//        break;
//    default:
//        return DefWindowProc(hWnd, message, wParam, lParam);
//    }
//    return 0;
//}

ATOM MyRegisterClass(HINSTANCE hInstance) {
    WNDCLASSEXW wcex = { sizeof(WNDCLASSEX) };
    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = WndProc;
    wcex.hInstance = hInstance;
    wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_TASKMANAGERGUICPP));
    wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wcex.lpszMenuName = MAKEINTRESOURCEW(IDC_TASKMANAGERGUICPP);
    wcex.lpszClassName = szWindowClass;
    wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));
    return RegisterClassExW(&wcex);
}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow) {
    hInst = hInstance;
    HWND hWnd = CreateWindowW(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, 0, 600, 500, nullptr, nullptr, hInstance, nullptr);

    if (!hWnd) {
        return FALSE;
    }

    // Создаем элементы управления
    CreateWindowW(L"STATIC", L"Название задачи:", WS_VISIBLE | WS_CHILD,
        10, 10, 100, 20, hWnd, nullptr, hInstance, nullptr);
    hNameEdit = CreateWindowW(L"EDIT", L"", WS_TABSTOP | WS_VISIBLE | WS_CHILD | WS_BORDER,
        120, 10, 200, 20, hWnd, (HMENU)IDC_NAME_EDIT, hInstance, nullptr);

    CreateWindowW(L"STATIC", L"Описание:", WS_VISIBLE | WS_CHILD,
        10, 40, 100, 20, hWnd, nullptr, hInstance, nullptr);
    hDescriptionEdit = CreateWindowW(L"EDIT", L"", WS_TABSTOP | WS_VISIBLE | WS_CHILD | WS_BORDER,
        120, 40, 200, 20, hWnd, (HMENU)IDC_DESCRIPTION_EDIT, hInstance, nullptr);

    CreateWindowW(L"STATIC", L"Статус:", WS_VISIBLE | WS_CHILD,
        10, 70, 100, 20, hWnd, nullptr, hInstance, nullptr);
    hStatusEdit = CreateWindowW(L"EDIT", L"", WS_TABSTOP | WS_VISIBLE | WS_CHILD | WS_BORDER,
        120, 70, 200, 20, hWnd, (HMENU)IDC_STATUS_EDIT, hInstance, nullptr);

    CreateWindowW(L"STATIC", L"Дата:", WS_VISIBLE | WS_CHILD,
        10, 100, 100, 20, hWnd, nullptr, hInstance, nullptr);
    hDateEdit = CreateWindowW(L"EDIT", L"", WS_TABSTOP | WS_VISIBLE | WS_CHILD | WS_BORDER,
        120, 100, 200, 20, hWnd, (HMENU)IDC_DATE_EDIT, hInstance, nullptr);

    CreateWindowW(L"BUTTON", L"Добавить", WS_TABSTOP | WS_VISIBLE | WS_CHILD | BS_DEFPUSHBUTTON,
        10, 130, 100, 30, hWnd, (HMENU)IDC_ADD_BUTTON, hInstance, nullptr);
    CreateWindowW(L"BUTTON", L"Удалить", WS_TABSTOP | WS_VISIBLE | WS_CHILD | BS_DEFPUSHBUTTON,
        120, 130, 100, 30, hWnd, (HMENU)IDC_DELETE_BUTTON, hInstance, nullptr);
    CreateWindowW(L"BUTTON", L"Просмотреть", WS_TABSTOP | WS_VISIBLE | WS_CHILD | BS_DEFPUSHBUTTON,
        230, 130, 100, 30, hWnd, (HMENU)IDC_VIEW_BUTTON, hInstance, nullptr);

    CreateWindowW(L"STATIC", L"Список задач:", WS_VISIBLE | WS_CHILD,
        10, 170, 100, 20, hWnd, nullptr, hInstance, nullptr);
    hListBox = CreateWindowW(L"LISTBOX", L"", WS_VISIBLE | WS_CHILD | WS_BORDER | LBS_NOTIFY,
        10, 190, 400, 200, hWnd, (HMENU)IDC_LISTBOX, hInstance, nullptr);

    task.printVtask(hListBox); // Инициализация списка задач

    ShowWindow(hWnd, nCmdShow);

    UpdateWindow(hWnd);
    return TRUE;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) {
    switch (message) {
    case WM_COMMAND: {
        int wmId = LOWORD(wParam);
        switch (wmId) {
        case IDC_ADD_BUTTON: {
            WCHAR name[100], description[100], status[100], date[20];
            GetWindowTextW(hNameEdit, name, 100);
            GetWindowTextW(hDescriptionEdit, description, 100);
            GetWindowTextW(hStatusEdit, status, 100);
            GetWindowTextW(hDateEdit, date, 20);

            std::wstring wName(name), wDescription(description), wStatus(status), wDate(date);
            std::string sName(wName.begin(), wName.end()), sDescription(wDescription.begin(), wDescription.end()),
                sStatus(wStatus.begin(), wStatus.end());
            double dDate = _wtof(date);

            task.aadTask(sName, sDescription, sStatus, dDate);
            task.printVtask(hListBox);
            break;
        }
        case IDC_DELETE_BUTTON: {
            int selectedIndex = (int)SendMessage(hListBox, LB_GETCURSEL, 0, 0);
            if (selectedIndex == LB_ERR) {
                MessageBoxA(hWnd, "Выберите задачу для удаления!", "Предупреждение", MB_OK);
                break;
            }

            WCHAR selectedTask[100];
            SendMessage(hListBox, LB_GETTEXT, selectedIndex, (LPARAM)selectedTask);
            std::wstring wTask(selectedTask);
            std::string sTask(wTask.begin(), wTask.end());
            task.delateTask(sTask);
            task.printVtask(hListBox);
            break;
        }
        case IDC_VIEW_BUTTON: {
            int selectedIndex = (int)SendMessage(hListBox, LB_GETCURSEL, 0, 0);
            if (selectedIndex == LB_ERR) {
                MessageBoxA(hWnd, "Выберите задачу для просмотра!", "Предупреждение", MB_OK);
                break;
            }

            WCHAR selectedTask[100];
            SendMessage(hListBox, LB_GETTEXT, selectedIndex, (LPARAM)selectedTask);
            std::wstring wTask(selectedTask);
            std::string sTask(wTask.begin(), wTask.end());
            task.printTask(sTask);
            break;
        }
        case IDM_ABOUT:
            DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
            break;
        case IDM_EXIT:
            DestroyWindow(hWnd);
            break;
        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
        }
        break;
    }
    case WM_PAINT: {
        PAINTSTRUCT ps;
        HDC hdc = BeginPaint(hWnd, &ps);
        EndPaint(hWnd, &ps);
        break;
    }
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam) {
    UNREFERENCED_PARAMETER(lParam);
    switch (message) {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;
    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL) {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}
