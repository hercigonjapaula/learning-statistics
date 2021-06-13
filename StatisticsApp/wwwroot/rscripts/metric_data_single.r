library(datasets)
library(graphics)
library(ggplot2)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)

## Skup podataka
data.name <- as.character(args[1])
variable <- as.numeric(args[2])
null.hypothesis <- as.numeric(args[3])
alternative.hypothesis <- as.character(args[4])
confidence.interval <- as.numeric(args[5])
test <- as.character(args[6])
data <- switch (data.name,
                "iris" = iris,
                "mtcars" = mtcars,
                "PlantGrowth" = PlantGrowth,
                "ToothGrowth" = ToothGrowth,
                read.csv(data.name))

## Test srednje vrijednosti / varijance populacije
switch(test,
       "mean" = t.test(data[,variable], mu = null.hypothesis, 
                       alternative =  alternative.hypothesis, 
                       conf.level = 0.95),
       "var" = sigma.test(data[,variable], sigmasq = null.hypothesis, 
                          alternative = alternative.hypothesis, 
                          conf.level = 0.95))

#ggplot(iris, aes(Sepal.Length, Sepal.Width)) + 
#  geom_point()
#ggsave("plot.png", path = plots.path)