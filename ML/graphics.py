import matplotlib.pyplot as plt
import pandas
import seaborn as sns

example_coofs = [1.105106, 0.6477862, 1.445153, 3.305237, 10.87852]


# exmple_tnveds = [345345, 435453, 334534, 425053, 453543]
class Graphic:

    def __init__(self, coofs: list):
        self.coofs = sorted(coofs)[::-1]
        self.tnveds = [1, 2, 3, 4, 5]
        # print(self.coofs)

    def paint(self):
        indexes = [1, 2, 3, 4, 5]
        plt.figure(figsize=(9, 3))
        # plt.subplot(1)
        plt.xlabel('Tnveds')
        plt.ylabel('Коэффициент')
        plt.bar(self.tnveds, self.coofs)
        plt.suptitle('Лучшие товары')
        plt.savefig('graphic.png')
        plt.show()
        # plt.savefig('graphic.png')

    def save_graphic(self):
        plt.savefig('graphic.png')


def main():
    graphic = Graphic(coofs=example_coofs)
    graphic.paint()
    # graphic.save_graphic()


if __name__ == '__main__':
    main()
