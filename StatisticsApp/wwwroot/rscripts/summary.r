library(datasets)
library(graphics)
suppressWarnings(require(raster,quietly = TRUE))

args = commandArgs(trailingOnly = TRUE)

## Skup podataka
data.name <- as.character(args[1])
variable <- as.numeric(args[2])
plot <- as.character(args[3])
data <- switch (data.name,
                "iris" = iris,
                "mtcars" = mtcars,
                "PlantGrowth" = PlantGrowth,
                "ToothGrowth" = ToothGrowth,
                read.csv(data.name)
)

## Izlaz
sink("C:/Users/Paula/Desktop/FER-10.semestar/StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/summary.txt")
print(summary(data[,variable]))
sink()