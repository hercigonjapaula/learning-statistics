library(datasets)
library(graphics)
library(ggplot2)
library(gginference)
require(TeachingDemos)
pdf(NULL)
args = commandArgs(trailingOnly = TRUE)

## Putanja
path = args[1]

## Skup podataka
data.name <- as.character(args[2])
variable <- as.numeric(args[3])
null.hypothesis <- as.numeric(args[4])
alternative.hypothesis <- as.character(args[5])
confidence.interval <- as.numeric(args[6])
test <- as.character(args[7])
data <- switch (data.name,
                "mtcars" = mtcars,
                "USArrests" = USArrests,
                read.csv(data.name))

## Test srednje vrijednosti / varijance populacije
if(test == "mean") {
  test.result <- t.test(data[,variable], mu = null.hypothesis, 
                        alternative =  alternative.hypothesis, 
                        conf.level = confidence.interval)
  cat(test.result$statistic, test.result$parameter, 
      test.result$p.value, test.result$conf.int, 
      test.result$estimate, sep = " ")
  ggttest(test.result, colaccept="lightsteelblue1", 
          colreject="grey84", colstat="navyblue")
  ggsave("test_plot.png", path = path)
} else if(test == "var") {
  test.result <- sigma.test(data[,variable], sigmasq = null.hypothesis, 
                            alternative = alternative.hypothesis, 
                            conf.level = confidence.interval)
  cat(test.result$statistic, test.result$parameter, 
      test.result$p.value, test.result$conf.int, 
      test.result$estimate, sep = " ")
  ggchisqtest(test.result, colaccept="lightsteelblue1", 
              colreject="grey84", colstat="navyblue")
  ggsave("test_plot.png", path = path)
}