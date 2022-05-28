//
// Created by LA on 2022/5/28.
//

#ifndef MINIDRAW_FREEHAND_H
#define MINIDRAW_FREEHAND_H

#include "shape.h"

class FreeHand : public Shape {
public:
    FreeHand();

    ~FreeHand();

public:
    void Draw(QPainter &painter);

private:
    QPainterPath path;
};


#endif //MINIDRAW_FREEHAND_H
