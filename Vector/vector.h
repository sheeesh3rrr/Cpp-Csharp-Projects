#ifndef VECTOR_H
#define VECTOR_H
#define VECTOR_MEMORY_IMPLEMENTED

#include <cstddef>
#include <new>
#include <memory>
#include <algorithm>
#include <stdexcept>
#include <iterator>

template <class T> class Vector {
public:
  using ValueType = T;
  using Pointer = T *;
  using ConstPointer = const T *;
  using Reference = T &;
  using ConstReference = const T &;
  using SizeType = size_t;
  using Iterator = T *;
  using ConstIterator = const T *;
  using ReverseIterator = std::reverse_iterator<Iterator>;
  using ConstReverseIterator = std::reverse_iterator<ConstIterator>;

private:
  T *arr_ = nullptr;
  size_t size_ = 0;
  size_t capacity_ = 0;

  T* PrepareBuffer() {
    return static_cast<T*>(operator new(size_ * sizeof(ValueType)));
  }

public:
  Vector() : arr_{nullptr}, size_{0}, capacity_{0} {}

  explicit Vector(size_t size) : arr_{nullptr}, size_{size}, capacity_{size} {
    if (size_ > 0) {
        arr_ = PrepareBuffer();
        size_t ready = 0;
        try {
            for (; ready < size_; ++ready) {
                new (arr_ + ready) T{};
            }
        } catch (...) {
            for (size_t i = 0; i < ready; ++i) {
                arr_[i].~T();
            }
            operator delete(arr_);
            throw;
        }
    }
  }

  Vector(size_t size, const T& value) : size_{size}, capacity_{size} {
    if (size_ > 0) {
        arr_ = PrepareBuffer();
        size_t ready = 0;
        try {
            for (; ready < size_; ++ready) {
                new (arr_ + ready) T{value};
            }
        } catch (...) {
            for (size_t i = 0; i < ready; ++i) {
                arr_[i].~T();
            }
            operator delete(arr_);
            throw;
        }
    }
  }

  template <class Iterator, class = std::enable_if_t<std::is_base_of_v<std::forward_iterator_tag, 
  typename std::iterator_traits<Iterator>::iterator_category>>>
  Vector(Iterator first, Iterator last) : arr_{nullptr}, size_{0}, capacity_{0} {
    size_t dist = std::distance(first, last);
    if (dist > 0){
        arr_ = static_cast<T*>(operator new(dist * sizeof(ValueType)));
        size_t ready = 0;
        try {
            for (; first != last; ++first) {
                new (arr_ + ready) T{*first};
                ready++;
            }
            size_ = dist;
            capacity_ = dist;
        } catch (...) {
            for (size_t i = 0; i < ready; ++i) {
                arr_[i].~T();
            }
            operator delete(arr_);
            throw;
        }
    }
  }

  Vector(std::initializer_list<T> lst) : size_{lst.size()}, capacity_{lst.size()} {
    if (lst.size() > 0) {
        arr_ = PrepareBuffer();
        size_t ready = 0;
        try {
            for (auto it = lst.begin(); it != lst.end(); ++it) {
                new (arr_ + ready) T{*it};
                ready++;
            }
        } catch (...) {
            for (size_t i = 0; i < ready; ++i) {
                arr_[i].~T();
            }
            operator delete(arr_);
            throw;
        }
    }
  }

  Vector(const Vector<T> &vec) : size_{vec.size_}, capacity_{vec.capacity_} {
    if (size_ > 0) {
      arr_ = PrepareBuffer();
      size_t ready = 0;
      try {
        for (; ready < size_; ++ready) {
          new (arr_ + ready) T{vec[ready]};
        }
      } catch (...) {
        for (size_t i = 0; i < ready; ++i) {
          arr_[i].~T();
        }
        operator delete(arr_);
        throw;
      }
    }
  }

  Vector(Vector<T> &&vec) noexcept
      : arr_{vec.arr_}, size_{vec.size_}, capacity_{vec.capacity_} {
    if (size_ > 0) {
      vec.arr_ = nullptr;
      vec.size_ = 0;
      vec.capacity_ = 0;
    }
  }

  ~Vector() {
    if (arr_) {
      std::destroy(arr_, arr_ + size_);
      operator delete(arr_);
    }
  }

  Vector &operator=(const Vector<T> &vec) {
    if (this != &vec) {
      Vector<T> tmp{vec};
      Swap(tmp);
    }
    return *this;
  }

  Vector &operator=(Vector<T> &&vec) noexcept {
    if (this == &vec) {
      return *this;
    }

    std::destroy(arr_, arr_ + size_);
    operator delete(arr_);

    capacity_ = vec.capacity_;
    size_ = vec.size_;
    arr_ = vec.arr_;
    vec.arr_ = nullptr;
    vec.size_ = 0;
    vec.capacity_ = 0;

    return *this;
  }

  size_t Size() const { return size_; }

  size_t Capacity() const { return capacity_; }

  bool Empty() const { return size_ == 0; }

  T &operator[](size_t idx) { return arr_[idx]; }

  const T &operator[](size_t idx) const { return arr_[idx]; }

  T &At(size_t idx) {
    if (idx >= size_) {
      throw std::out_of_range{"Index out of range"};
    }

    return arr_[idx];
  }

  const T &At(size_t idx) const {
    if (idx >= size_) {
      throw std::out_of_range{"Index out of range"};
    }

    return arr_[idx];
  }

  T &Front() {
    if (size_ == 0 || arr_ == nullptr) {
      throw std::out_of_range{"Index out of range"};
    }
    return arr_[0];
  }

  T &Back() {
    if (size_ == 0 || arr_ == nullptr) {
      throw std::out_of_range{"Index out of range"};
    }
    return arr_[size_ - 1];
  }

  const T &Front() const {
    if (size_ == 0 || arr_ == nullptr) {
      throw std::out_of_range{"Index out of range"};
    }
    return arr_[0];
  }

  const T &Back() const {
    if (size_ == 0 || arr_ == nullptr) {
      throw std::out_of_range{"Index out of range"};
    }
    return arr_[size_ - 1];
  }

  T *Data() { return arr_; }

  const T *Data() const { return arr_; }

  void Swap(Vector<T> &other) {
    std::swap(size_, other.size_);
    std::swap(capacity_, other.capacity_);
    std::swap(arr_, other.arr_);
  }

  void Resize(size_t new_size) {
    if (new_size < size_) {
      std::destroy(arr_ + new_size, arr_ + size_);
      size_ = new_size;

    } else if (new_size == size_) {
      return;

    } else if (new_size <= capacity_) {
      size_t ready = 0;
      try {
        for (size_t i = size_; i < new_size; ++i) {
          new (arr_ + i) T{};
          ready++;
        }
      } catch (...) {
        for (size_t i = size_; i < ready; ++i) {
          arr_[i].~T();
        }
        throw;
      }
      size_ = new_size;

    } else {
      auto new_arr = static_cast<T*>(operator new(new_size * sizeof(ValueType)));
      size_t ready = 0;

      try {
        for (; ready < size_; ++ready) {
          new (new_arr + ready) T(std::move(arr_[ready]));
        }
        for (; ready < new_size; ++ready) {
          new (new_arr + ready) T{};
        }
      } catch (...) {
        for (size_t i = 0; i < ready; ++i) {
          new_arr[i].~T();
        }
        operator delete(new_arr);
        throw;
      }

      std::destroy(arr_, arr_ + size_);
      operator delete(arr_);

      arr_ = new_arr;
      capacity_ = new_size;
      size_ = new_size;
    }
  }

  void Resize(size_t new_size, T value) {
    if (new_size < size_) {
      std::destroy(arr_ + new_size, arr_ + size_);
      size_ = new_size;

    } else if (new_size == size_) {
      return;

    } else if (new_size <= capacity_) {
      size_t ready = 0;
      try {
        for (size_t i = size_; i < new_size; ++i) {
          new (arr_ + i) T{value};
          ready++;
        }
      } catch (...) {
        for (size_t i = size_; i < ready; ++i) {
          arr_[i].~T();
        }
        throw;
      }
      size_ = new_size;
      
    } else {
      auto new_arr = static_cast<T*>(operator new(new_size * sizeof(ValueType)));
      size_t ready = 0;

      try {
        for (; ready < size_; ++ready) {
          new (new_arr + ready) T(std::move(arr_[ready]));
        }
        for (; ready < new_size; ++ready) {
          new (new_arr + ready) T{value};
        }
      } catch (...) {
        for (size_t i = 0; i < ready; ++i) {
          new_arr[i].~T();
        }
        operator delete(new_arr);
        throw;
      }

      std::destroy(arr_, arr_ + size_);
      operator delete(arr_);

      arr_ = new_arr;
      capacity_ = new_size;
      size_ = new_size;
    }
  }

  void Reserve(size_t new_cap) {
    if (new_cap <= capacity_) {
      return;
    }
    if (new_cap == 0) {
      Clear();
      operator delete(arr_);
      arr_ = nullptr;
      capacity_ = new_cap;
      return;
    }

    auto new_arr = static_cast<T*>(operator new (new_cap * sizeof(ValueType)));
    size_t ready = 0;
    try {
      for (; ready < size_; ++ready) {
        new (new_arr + ready) T{std::move(arr_[ready])};
      }
    } catch (...) {
      for (size_t i = 0; i < ready; ++i) {
        new_arr[i].~T();
      }
      operator delete(new_arr);
      throw;
    }

    std::destroy(arr_, arr_ + size_);
    operator delete(arr_);

    arr_ = new_arr;
    capacity_ = new_cap;
  }

  void ShrinkToFit() {
    if (size_ >= capacity_) {
      return;
    }

    if (size_ == 0) {
      Clear();
      operator delete(arr_);
      arr_ = nullptr;
      capacity_ = 0;
      return;
    }

    auto new_arr = static_cast<T*>(operator new(size_ * sizeof(ValueType)));
    size_t ready = 0;
    try {
      for (; ready < size_; ++ready) {
        new (new_arr + ready) T{std::move(arr_[ready])};
      }
    } catch (...) {
      for (size_t i = 0; i < ready; ++i) {
        new_arr[i].~T();
      }
      operator delete(new_arr);
      throw;
    }

    std::destroy(arr_, arr_ + size_);
    operator delete(arr_);

    arr_ = new_arr;
    capacity_ = size_;
  }

  void Clear() {
    if (arr_) {
      std::destroy(arr_, arr_ + size_);
      size_ = 0;
    }
  }

  void PushBack(const T &val) {
    T to_add = val;
    if (size_ >= capacity_) {
        Reserve(capacity_ == 0 ? 1 : 2 * capacity_);
    }
    new (arr_ + size_) T{to_add};
    size_++;
  }

  void PushBack(T &&val) {
    if (size_ < capacity_) {
      new (arr_ + size_) T{std::move(val)};
      size_++;
      return;
    }

    auto new_arr = static_cast<T*>(operator new(
      (capacity_ == 0 ? 1 : 2 * capacity_) * sizeof(ValueType)));
    size_t ready = 0;
    try {
      for (; ready < size_; ++ready) {
        new (new_arr + ready) T{std::move(arr_[ready])};
      }
      new (new_arr + ready) T{std::move(val)};
    } catch (...) {
      for (size_t i = 0; i < ready; ++i) {
        new_arr[i].~T();
      }
      operator delete(new_arr);
      throw;
    }

    std::destroy(arr_, arr_ + size_);
    operator delete(arr_);

    arr_ = new_arr;
    capacity_ = capacity_ == 0 ? 1 : 2 * capacity_;
    size_++;
  }

  void PopBack() {
    if (size_ > 0) {
      std::destroy_at(arr_ + size_ - 1);
      --size_;
    }
  }

  template <typename... Args>
  void EmplaceBack(Args&&... args) {
    if (size_ >= capacity_) {
      Reserve(capacity_ == 0 ? 1 : 2 * capacity_);
    }
    new (arr_ + size_) T(std::forward<Args>(args)...);
    ++size_;
  }

  bool operator<(const Vector &other) const {
    for (size_t i = 0; i < std::min(size_, other.size_); ++i) {
      if (arr_[i] == other[i]) {
        continue;
      }
      return arr_[i] < other[i];
    }
    return size_ < other.size_;
  }

  bool operator>(const Vector &other) const {
    for (size_t i = 0; i < std::min(size_, other.size_); ++i) {
      if (arr_[i] == other[i]) {
        continue;
      }
      return arr_[i] > other[i];
    }
    return size_ > other.size_;
  }

  bool operator<=(const Vector &other) const { return !(*this > other); }

  bool operator>=(const Vector &other) const { return !(*this < other); }

  bool operator==(const Vector &other) const {
    if (size_ != other.size_) {
      return false;
    }
    for (size_t i = 0; i < size_; ++i) {
      if (arr_[i] != other[i]) {
        return false;
      }
    }
    return true;
  }

  bool operator!=(const Vector &other) const { return !(*this == other); }

  T *begin() { // NOLINT
    return arr_;
  }

  T *end() { // NOLINT
    return arr_ + size_;
  }

  const T *begin() const { // NOLINT
    return arr_;
  }

  const T *end() const { // NOLINT
    return arr_ + size_;
  }

  const T *cbegin() const { // NOLINT
    return arr_;
  }

  const T *cend() const { // NOLINT
    return arr_ + size_;
  }

  ReverseIterator rbegin() {       // NOLINT
    return ReverseIterator(end()); // NOLINT
  }

  ReverseIterator rend() {           // NOLINT
    return ReverseIterator(begin()); // NOLINT
  }

  ConstReverseIterator rbegin() const { // NOLINT
    return ConstReverseIterator(end()); // NOLINT
  }

  ConstReverseIterator rend() const {     // NOLINT
    return ConstReverseIterator(begin()); // NOLINT
  }

  ConstReverseIterator crbegin() const { // NOLINT
    return ConstReverseIterator(end());  // NOLINT
  }

  ConstReverseIterator crend() const {    // NOLINT
    return ConstReverseIterator(begin()); // NOLINT
  }
};
#endif // VECTOR_H