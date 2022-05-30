/****************************************************************************
** Meta object code from reading C++ file 'ImageWidget.h'
**
** Created by: The Qt Meta Object Compiler version 67 (Qt 5.15.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include <memory>
#include "../../../../../src/App/ImageWidget.h"
#include <QtCore/qbytearray.h>
#include <QtCore/qmetatype.h>
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'ImageWidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 67
#error "This file was generated using the moc from 5.15.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
QT_WARNING_PUSH
QT_WARNING_DISABLE_DEPRECATED
struct qt_meta_stringdata_ImageWidget_t {
    QByteArrayData data[22];
    char stringdata0[190];
};
#define QT_MOC_LITERAL(idx, ofs, len) \
    Q_STATIC_BYTE_ARRAY_DATA_HEADER_INITIALIZER_WITH_OFFSET(len, \
    qptrdiff(offsetof(qt_meta_stringdata_ImageWidget_t, stringdata0) + ofs \
        - idx * sizeof(QByteArrayData)) \
    )
static const qt_meta_stringdata_ImageWidget_t qt_meta_stringdata_ImageWidget = {
    {
QT_MOC_LITERAL(0, 0, 11), // "ImageWidget"
QT_MOC_LITERAL(1, 12, 4), // "Open"
QT_MOC_LITERAL(2, 17, 0), // ""
QT_MOC_LITERAL(3, 18, 4), // "Save"
QT_MOC_LITERAL(4, 23, 6), // "SaveAs"
QT_MOC_LITERAL(5, 30, 6), // "Invert"
QT_MOC_LITERAL(6, 37, 6), // "Mirror"
QT_MOC_LITERAL(7, 44, 10), // "horizontal"
QT_MOC_LITERAL(8, 55, 8), // "vertical"
QT_MOC_LITERAL(9, 64, 8), // "TurnGray"
QT_MOC_LITERAL(10, 73, 7), // "Restore"
QT_MOC_LITERAL(11, 81, 6), // "Choose"
QT_MOC_LITERAL(12, 88, 3), // "IDW"
QT_MOC_LITERAL(13, 92, 3), // "RBF"
QT_MOC_LITERAL(14, 96, 3), // "Fix"
QT_MOC_LITERAL(15, 100, 11), // "Convolution"
QT_MOC_LITERAL(16, 112, 4), // "Undo"
QT_MOC_LITERAL(17, 117, 15), // "mousePressEvent"
QT_MOC_LITERAL(18, 133, 12), // "QMouseEvent*"
QT_MOC_LITERAL(19, 146, 10), // "mouseevent"
QT_MOC_LITERAL(20, 157, 14), // "mouseMoveEvent"
QT_MOC_LITERAL(21, 172, 17) // "mouseReleaseEvent"

    },
    "ImageWidget\0Open\0\0Save\0SaveAs\0Invert\0"
    "Mirror\0horizontal\0vertical\0TurnGray\0"
    "Restore\0Choose\0IDW\0RBF\0Fix\0Convolution\0"
    "Undo\0mousePressEvent\0QMouseEvent*\0"
    "mouseevent\0mouseMoveEvent\0mouseReleaseEvent"
};
#undef QT_MOC_LITERAL

static const uint qt_meta_data_ImageWidget[] = {

 // content:
       8,       // revision
       0,       // classname
       0,    0, // classinfo
      18,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: name, argc, parameters, tag, flags
       1,    0,  104,    2, 0x0a /* Public */,
       3,    0,  105,    2, 0x0a /* Public */,
       4,    0,  106,    2, 0x0a /* Public */,
       5,    0,  107,    2, 0x0a /* Public */,
       6,    2,  108,    2, 0x0a /* Public */,
       6,    1,  113,    2, 0x2a /* Public | MethodCloned */,
       6,    0,  116,    2, 0x2a /* Public | MethodCloned */,
       9,    0,  117,    2, 0x0a /* Public */,
      10,    0,  118,    2, 0x0a /* Public */,
      11,    0,  119,    2, 0x0a /* Public */,
      12,    0,  120,    2, 0x0a /* Public */,
      13,    0,  121,    2, 0x0a /* Public */,
      14,    0,  122,    2, 0x0a /* Public */,
      15,    0,  123,    2, 0x0a /* Public */,
      16,    0,  124,    2, 0x0a /* Public */,
      17,    1,  125,    2, 0x0a /* Public */,
      20,    1,  128,    2, 0x0a /* Public */,
      21,    1,  131,    2, 0x0a /* Public */,

 // slots: parameters
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void, QMetaType::Bool, QMetaType::Bool,    7,    8,
    QMetaType::Void, QMetaType::Bool,    7,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void, 0x80000000 | 18,   19,
    QMetaType::Void, 0x80000000 | 18,   19,
    QMetaType::Void, 0x80000000 | 18,   19,

       0        // eod
};

void ImageWidget::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        auto *_t = static_cast<ImageWidget *>(_o);
        Q_UNUSED(_t)
        switch (_id) {
        case 0: _t->Open(); break;
        case 1: _t->Save(); break;
        case 2: _t->SaveAs(); break;
        case 3: _t->Invert(); break;
        case 4: _t->Mirror((*reinterpret_cast< bool(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2]))); break;
        case 5: _t->Mirror((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 6: _t->Mirror(); break;
        case 7: _t->TurnGray(); break;
        case 8: _t->Restore(); break;
        case 9: _t->Choose(); break;
        case 10: _t->IDW(); break;
        case 11: _t->RBF(); break;
        case 12: _t->Fix(); break;
        case 13: _t->Convolution(); break;
        case 14: _t->Undo(); break;
        case 15: _t->mousePressEvent((*reinterpret_cast< QMouseEvent*(*)>(_a[1]))); break;
        case 16: _t->mouseMoveEvent((*reinterpret_cast< QMouseEvent*(*)>(_a[1]))); break;
        case 17: _t->mouseReleaseEvent((*reinterpret_cast< QMouseEvent*(*)>(_a[1]))); break;
        default: ;
        }
    }
}

QT_INIT_METAOBJECT const QMetaObject ImageWidget::staticMetaObject = { {
    QMetaObject::SuperData::link<QWidget::staticMetaObject>(),
    qt_meta_stringdata_ImageWidget.data,
    qt_meta_data_ImageWidget,
    qt_static_metacall,
    nullptr,
    nullptr
} };


const QMetaObject *ImageWidget::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->dynamicMetaObject() : &staticMetaObject;
}

void *ImageWidget::qt_metacast(const char *_clname)
{
    if (!_clname) return nullptr;
    if (!strcmp(_clname, qt_meta_stringdata_ImageWidget.stringdata0))
        return static_cast<void*>(this);
    return QWidget::qt_metacast(_clname);
}

int ImageWidget::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 18)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 18;
    } else if (_c == QMetaObject::RegisterMethodArgumentMetaType) {
        if (_id < 18)
            *reinterpret_cast<int*>(_a[0]) = -1;
        _id -= 18;
    }
    return _id;
}
QT_WARNING_POP
QT_END_MOC_NAMESPACE
