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
mean <- mean(data[,variable])
# medijan
median <- median(data[,variable])
# mod
mfv <- mfv(data[,variable])

## Mjere rasipanja
# rang
rang <- max(data[,variable])-min(data[,variable])
# interkvartilni rang
iqr <- IQR(data[,variable])
# varijanca
var <- var(data[,variable])
# standardna devijacija
stddev <- sd(data[,variable])

## Izlaz
sink("C:/Users/Paula/Desktop/FER-10.semestar/output.txt")
print(summary(data[,variable]))
sink()