library(datasets)
library(graphics)
library(modeest)
library(ggplot2)

suppressWarnings(require(raster,quietly = TRUE))
pdf(NULL)
args = commandArgs(trailingOnly = TRUE)

## Putanje
# Putanja za defaultne plotove
root.path = args[1]
# Putanja za korisnikove plotove
plots.path = args[2]

## Skup podataka
data.name <- as.character(args[3])
variable <- as.numeric(args[4])
plot <- as.character(args[5])
data <- switch (data.name,
  "iris" = iris,
  "mtcars" = mtcars,
  "PlantGrowth" = PlantGrowth,
  "ToothGrowth" = ToothGrowth,
  read.csv(data.name)
)

## Osnovne vizualizacije
# numerièke varijable
numerical.columns <- unlist(lapply(data, is.numeric))
png(file=paste(root.path, "basic_plots/boxplot.png", sep = ""))
boxplot(data[,numerical.columns], 
        main = "Pravokutni dijagrami numerièkih varijabli")
dev.off()
# kategorijske varijable
factor.columns <- names(Filter(is.factor, data))
for (col in factor.columns) {
  col.freq = table(data[,col])
  png(file=paste(root.path, "basic_plots/", col, "_barplot.png", sep = ""))
  barplot(col.freq, 
          main = paste("Stupèasti dijagram kategorièke varijable", col, sep = " "))
  dev.off()
}

## Grafovi
# histogram, box plot, scatter plot
png(file=paste(root.path, "plot.png", sep = ""))
switch(plot,
         "histogram" = hist(x = data[,variable], breaks = 25, 
                            main = paste(names(data)[variable], 'histogram', sep = " "),
                            xlab = names(data)[variable], ylab = 'Frequency'),
         "boxplot" = boxplot(x = data[,variable], 
                             main = paste(names(data)[variable], 'box plot', sep = " "),
                             ylab = names(data)[variable]),
         "scatterplot" = plot(x = data[,variable], 
                              main = paste(names(data)[variable], 'scatter plot', sep = " "),
                              ylab = names(data)[variable])
         )
dev.off()

## Mjere centralne tendencije
# aritmetièka sredina
mean <- round(mean(data[,variable]), 2)
# medijan
median <- round(median(data[,variable]), 2)
# mod
mfv <- round(mfv(data[,variable]), 2)

## Mjere rasipanja
# rang
rang <- round(max(data[,variable])-min(data[,variable]), 2)
# interkvartilni rang
iqr <- round(IQR(data[,variable]), 2)
# varijanca
var <- round(var(data[,variable]), 2)
# standardna devijacija
stddev <- round(sd(data[,variable]), 2)

## Ostale mjere
# minimum
minimum <- min(data[,variable])
# maksimum
maximum <- max(data[,variable])
# 1. kvartil
first.quartile <- round(summary(data[,variable])[2], 2)
# 3. kvartil
third.quartile <- round(summary(data[,variable])[5], 2)

## Izlaz
sink(paste(root.path, "descriptive_statistics.txt", sep = ""))
cat(paste(mean, median, mfv, 
            rang, iqr, var, stddev, 
            minimum, maximum, 
            first.quartile, third.quartile, 
            sep = " "))
sink()

#ggplot(iris, aes(Sepal.Length, Sepal.Width)) + 
#  geom_point()
#ggsave("plot.png", path = plots.path)