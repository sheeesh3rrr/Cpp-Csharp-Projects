#ifndef MATRIX_H
#define MATRIX_H
#define MATRIX_SQUARE_MATRIX_IMPLEMENTED

#include <iostream>
#include <stdexcept>
#include <cmath>

class MatrixIsDegenerateError : public std::runtime_error {
public:
  MatrixIsDegenerateError() : std::runtime_error("MatrixIsDegenerateError") {}
};

class MatrixOutOfRange : public std::out_of_range {
public:
  MatrixOutOfRange() : std::out_of_range("MatrixOutOfRange") {}
};

template <typename T, std::size_t Rows, std::size_t Columns> class Matrix {
public:
  T matr[Rows][Columns];

  std::size_t RowsNumber() const { return Rows; }
  std::size_t ColumnsNumber() const { return Columns; }

  T &operator()(const std::size_t &r, const std::size_t &c) {
    return matr[r][c];
  }

  const T &operator()(const std::size_t &r, const std::size_t &c) const {
    return matr[r][c];
  }

  T &At(const std::size_t &r, const std::size_t &c) {
    if (r >= Rows || c >= Columns) {
      throw MatrixOutOfRange{};
    }
    return matr[r][c];
  }

  const T &At(const std::size_t &r, const std::size_t &c) const {
    if (r >= Rows || c >= Columns) {
      throw MatrixOutOfRange{};
    }
    return matr[r][c];
  }

  Matrix<T, Rows, Columns> operator+(const Matrix &other) const {
    Matrix<T, Rows, Columns> ans;

    for (std::size_t i = 0; i < Rows; ++i) {
      for (std::size_t j = 0; j < Columns; ++j) {
        ans(i, j) = matr[i][j] + other(i, j);
      }
    }

    return ans;
  }

  Matrix<T, Rows, Columns> operator-(const Matrix &other) const {
    Matrix<T, Rows, Columns> ans;

    for (std::size_t i = 0; i < Rows; ++i) {
      for (std::size_t j = 0; j < Columns; ++j) {
        ans(i, j) = matr[i][j] - other(i, j);
      }
    }

    return ans;
  }

  template <std::size_t Cols>
  Matrix<T, Rows, Cols> operator*(const Matrix<T, Columns, Cols> &other) const {
    Matrix<T, Rows, Cols> ans;
    for (std::size_t i = 0; i < Rows; ++i) {
      for (std::size_t j = 0; j < Cols; ++j) {
        ans(i, j) = T();
        for (std::size_t pos = 0; pos < Columns; ++pos) {
          ans(i, j) += matr[i][pos] * other(pos, j);
        }
      }
    }
    return ans;
  }

  Matrix<T, Rows, Columns> operator*(const T& num) const {
    Matrix<T, Rows, Columns> ans;
    for (std::size_t i = 0; i < Rows; ++i) {
      for (std::size_t j = 0; j < Columns; ++j) {
        ans(i, j) = matr[i][j] * num;
      }
    }
    return ans;
  }

  friend Matrix<T, Rows, Columns> operator*(const T& num, const Matrix<T, Rows, Columns>& m) {
    return m * num;
  }

  Matrix<T, Rows, Columns> operator/(const T &num) const {
    if (num == static_cast<T>(0)) {
      throw MatrixIsDegenerateError();
    }
    Matrix<T, Rows, Columns> ans;
    for (std::size_t i = 0; i < Rows; ++i) {
      for (std::size_t j = 0; j < Columns; ++j) {
        ans(i, j) = matr[i][j] / num;
      }
    }
    return ans;
  }

  Matrix<T, Rows, Columns> &operator+=(const Matrix &other) {
    *this = *this + other;
    return *this;
  }

  Matrix<T, Rows, Columns> &operator-=(const Matrix &other) {
    *this = *this - other;
    return *this;
  }

  Matrix<T, Rows, Columns>& operator*=(const Matrix<T, Columns, Columns> &other) {
    *this = *this * other;
    return *this;
  }

  Matrix<T, Rows, Columns>& operator*=(const T& num) {
    *this = *this * num;
    return *this;
  }

  Matrix<T, Rows, Columns> &operator/=(const T &num) {
    if (num == static_cast<T>(0)) {
      throw MatrixIsDegenerateError();
    }
    *this = *this / num;
    return *this;
  }

  bool operator==(const Matrix<T, Rows, Columns> &other) const {
    for (std::size_t i = 0; i < Rows; ++i) {
      for (std::size_t j = 0; j < Columns; ++j) {
        if (matr[i][j] != other(i, j)) {
          return false;
        }
      }
    }
    return true;
  }

  bool operator!=(const Matrix<T, Rows, Columns> &other) const {
    return !(*this == other);
  }

  friend std::ostream &operator<<(std::ostream &os, const Matrix<T, Rows, Columns> &m) {
    for (std::size_t i = 0; i < Rows; ++i) {
      for (std::size_t j = 0; j < Columns; ++j) {
        if (j == Columns - 1) {
          os << m(i, j);
        } else {
          os << m(i, j) << ' ';          
        }
      }
      os << '\n';
    }
    return os;
  };

  friend std::istream &operator>>(std::istream &is, Matrix<T, Rows, Columns> &m) {
    for (std::size_t i = 0; i < Rows; ++i) {
      for (std::size_t j = 0; j < Columns; ++j) {
        is >> m(i, j);
      }
    }
    return is;
  };
};

template <typename T, std::size_t Rows, std::size_t Columns>
Matrix<T, Columns, Rows> GetTransposed(const Matrix<T, Rows, Columns> m) {
  Matrix<T, Columns, Rows> ans;
  for (std::size_t i = 0; i < Columns; ++i) {
    for (std::size_t j = 0; j < Rows; ++j) {
      ans(i, j) = m(j, i);
    }
  }
  return ans;
}

template<typename T, std::size_t Rows, std::size_t Columns>
void Transpose(Matrix<T, Rows, Columns>& m) {
  Matrix<T, Columns, Rows> new_matr;
  for (std::size_t i = 0; i < Rows; ++i) {
    for (std::size_t j = 0; j < Columns; ++j) {
      new_matr(j, i) = m(i, j);
    }
  }
  m = new_matr;
}

template<typename T, std::size_t Dims>
T Trace(Matrix<T, Dims, Dims> m) {
  T ans = 0;
  for (std::size_t i = 0; i < Dims; ++i) {
    ans += m(i, i);
  }
  return ans;
}

template<typename T>
T Determinant(Matrix<T, 0, 0>) {
  return T(1);
}

template<typename T>
T Determinant(Matrix<T, 1, 1> m) {
  return m(0, 0);
}

template<typename T>
T Determinant(Matrix<T, 2, 2> m) {
  return m(0, 0) * m(1, 1) - m(0, 1) * m(1, 0);
}

template<typename T, std::size_t Dims>
T Determinant(Matrix<T, Dims, Dims> m) {
  T ans = 0;
  for (std::size_t k = 0; k < Dims; ++k) {
    ans += (k % 2 == 0 ? static_cast<T>(1) : static_cast<T>(-1)) * m(0, k) * Determinant(CreateMinor(m, 0, k));
  }
  return ans;
}

template<typename T, std::size_t Dims>
Matrix<T, Dims, Dims> GetInversed(Matrix<T, Dims, Dims> m) {
  if (Determinant(m) == 0) {
    throw MatrixIsDegenerateError();
  }
  return CreateMinorMatrix(m) / Determinant(m);
}

template<typename T>
Matrix<T, 1, 1> GetInversed(Matrix<T, 1, 1> m) {
  if (m(0, 0) == T(0)) {
    throw MatrixIsDegenerateError();
  }
  Matrix<T, 1, 1> ans;
  ans(0, 0) = T(1) / m(0, 0);
  return ans;
}

template<typename T, std::size_t Dims>
void Inverse(Matrix<T, Dims, Dims>& m) {
  if (Determinant(m) == 0) {
    throw MatrixIsDegenerateError();
  }
  m = GetInversed(m);
}

template<typename T, std::size_t Dims>
Matrix<T, Dims - 1, Dims - 1> CreateMinor(Matrix<T, Dims, Dims> m, std::size_t req_i, std::size_t req_j) {
  Matrix<T, Dims - 1, Dims - 1> ans;
  std::size_t cord_i = 0;
  std::size_t cord_j = 0;
  for (std::size_t i = 0; i < Dims; ++i) {
    if (i == req_i) {
      continue;
    }
    cord_j = 0;
    for (std::size_t j = 0; j < Dims; ++j) {
      if (j == req_j) {
        continue;
      }
      ans(cord_i, cord_j) = m(i, j);
      ++cord_j;
    }
    ++cord_i;
  }
  return ans;
}

template<typename T, std::size_t Dims>
Matrix<T, Dims, Dims> CreateMinorMatrix(Matrix<T, Dims, Dims> m) {
  Matrix<T, Dims, Dims> ans;
  for (std::size_t i = 0; i < Dims; ++i) {
    for (std::size_t j = 0; j < Dims; ++j) {
      ans(i, j) = Determinant(CreateMinor(m, i, j)) * ((i + j) % 2 == 0 ? T(1) : T(-1));
    }
  }
  return GetTransposed(ans);
}

#endif // MATRIX_H