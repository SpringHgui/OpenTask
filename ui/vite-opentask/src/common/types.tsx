
// 接收一个参数T，无返回值的函数委托
export type Action<T> = (arg: T) => void;

// 接收一个参数T，返回R的函数委托
export type Func<T, R> = (arg: T) => R;

