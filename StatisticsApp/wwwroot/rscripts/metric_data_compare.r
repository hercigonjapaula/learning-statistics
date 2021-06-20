library(datasets)
library(graphics)
library(ggplot2)
library(stats)
library(gginference)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
pdf(NULL)
path = args[1]

## Skup podataka
data1 <- scan(as.character(args[2]))
data2 <- scan(as.character(args[3]))
alternative.hypothesis <- as.character(args[4])
confidence.interval <- as.numeric(args[5])
test <- as.character(args[6])

## Box plot 
png(file=paste(path, "boxplot.png", sep = "/"))
boxplot(data1, data2, main = "Pravokutni dijagrami obje populacije",
        names = c("Prvi skup podataka", "Drugi skup podataka"))
dev.off()

## Test o jednakosti srednjih vrijednosti / varijanci dviju populacija
if(test == "mean"){
  test.result <- t.test(data1, data2, 
                        alternative = alternative.hypothesis,
                        conf.level = confidence.interval,
                        var.equal = TRUE)  
  cat(test.result$statistic, test.result$parameter, 
      test.result$p.value, test.result$conf.int, 
      test.result$estimate, sep = " ")  
  ggttest(test.result, colaccept="lightsteelblue1", 
          colreject="grey84", colstat="navyblue")
  ggsave("test_plot.png", path = path)
} else if(test == "var"){
  test.result <- var.test(data1, data2, 
                          alternative = alternative.hypothesis,
                          conf.level = confidence.interval)
  cat(test.result$statistic, test.result$parameter, 
      test.result$p.value, test.result$conf.int, 
      test.result$estimate, sep = " ")  
  ggvartest(test.result, colaccept="lightsteelblue1", 
            colreject="grey84", colstat="navyblue")
  ggsave("test_plot.png", path = path)
}