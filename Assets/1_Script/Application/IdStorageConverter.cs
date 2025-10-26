using System;

public static class StorageConverter
{
    // TIn을 TOut으로 변환하는 Func를 받아 모든 Slot에 적용 후 리턴
    public static SlotStorage<TOut> ConvertStorage<TIn, TOut>(SlotStorage<TIn> source, Func<TIn, TOut> selector)
    {
        var result = new SlotStorage<TOut>();

        foreach (var slot in source.GetAllSlotDatas())
        {
            var converted = selector(source.GetSlot(slot));
            result.AddSlot(slot.Team, converted);
        }

        return result;
    }
}