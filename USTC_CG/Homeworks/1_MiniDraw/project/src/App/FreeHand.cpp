//
// Created by LA on 2022/5/28.
//

#include "FreeHand.h"

FreeHand::FreeHand() {};

FreeHand::~FreeHand() {};

void FreeHand::Draw(QPainter &painter) {
    if (path.elementCount() == 0)
        path.moveTo(end);
    else
        path.lineTo(end);
    painter.drawPath(path);
}