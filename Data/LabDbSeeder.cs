using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Data;

public static class LabDbSeeder
{
    public static async Task SeedAsync(LabDbContext db)
    {
        if (await db.Teachers.AnyAsync() || await db.Works.AnyAsync() ||
            await db.Classrooms.AnyAsync() || await db.Reports.AnyAsync())
            return;

        db.Teachers.AddRange(
            new Teacher(0, "Иванова И. И.", "Физика"),
            new Teacher(0, "Петров П. П.", "Электроника"));
        db.Works.AddRange(
            new LabWork(0, "Закон Ома", "Соберите цепь, измерьте напряжение и ток, рассчитайте сопротивление.", "Физика", "Мультиметр", 5),
            new LabWork(0, "Измерение сигнала", "Подключите осциллограф и измерьте амплитуду сигнала.", "Электроника", "Осциллограф", 3));
        db.Classrooms.AddRange(
            new Classroom(0, "101", ["Мультиметр", "Осциллограф"]),
            new Classroom(0, "202", ["Мультиметр"]));
        await db.SaveChangesAsync();
    }
}
