using Project.Core;
using Project.Core.Domain.Catalog;
using Project.Data;
using Project.Services.Common.DataModel;

namespace Project.Services.Catalog;

/// <summary>
/// Represents department service
/// </summary>
public class DepartmentService : IDepartmentService
{
    #region Fields

    private readonly IRepository<Department> _departmentRepository;

    #endregion

    #region Constructor

    public DepartmentService(IRepository<Department> departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get department select list
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the department list
    /// </returns>
    public async Task<IList<SelectListDataModel>> GetSelectDataModelListAsync()
    {
        var query = from d in _departmentRepository.Table
                    where d.Active && d.Deleted == false
                    select new SelectListDataModel
                    {
                        Id = d.Id,
                        Name = d.Name
                    };

        return await query.ToListAsync();
    }

    /// <summary>
    /// Gets a department by ID
    /// </summary>
    /// <param name="departmentId">Department identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the Department
    /// </returns>
    public async Task<Department> GetDepartmentByIdAsync(long departmentId)
    {
        return await _departmentRepository.GetByIdAsync(departmentId);
    }

    /// <summary>
    /// Gets a departments by identifier list
    /// </summary>
    /// <param name="departmentIds">Department identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the department list
    /// </returns>
    public async Task<IList<Department>> GetDepartmentsByIdsAsync(IList<long> departmentIds)
    {
        return await _departmentRepository.GetByIdsAsync(departmentIds);
    }

    /// <summary>
    /// Check whether a department code is already exist or not
    /// </summary>
    /// <param name="code">Department code</param>
    /// <param name="departmentId">Department identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    public async Task<bool> HasDepartmentCodeAsync(string code, long departmentId = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code, nameof(code));

        var query = _departmentRepository.Table;
        if (departmentId != 0)
            query = query.Where(l => l.Id != departmentId);

        var department = await query.AnyAsync(l => l.Code == code && l.Deleted == false);

        return department;
    }

    /// <summary>
    /// Insert department
    /// </summary>
    /// <param name="department">Department</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the Department
    /// </returns>
    public async Task InsertDepartmentAsync(Department department)
    {
        await _departmentRepository.InsertAsync(department);
    }

    /// <summary>
    /// Update department
    /// </summary>
    /// <param name="department">Department</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the Department
    /// </returns>
    public async Task UpdateDepartmentAsync(Department department)
    {
        await _departmentRepository.UpdateAsync(department);
    }

    /// <summary>
    /// Delete department
    /// </summary>
    /// <param name="department">Department</param>
    /// <returns></returns>
    public async Task DeleteDepartmentAsync(Department department)
    {
        await _departmentRepository.DeleteAsync(department);
    }

    /// <summary>
    /// Gets all departments
    /// </summary>
    /// <param name="departmentName">Department name; null to load all records</param>
    /// <param name="departmentCode">Department code; null to load all records</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the departments
    /// </returns>
    public async Task<IPagedList<Department>> GetAllDepartmentsAsync(string departmentName = null,
         string departmentCode = null,
         int pageIndex = 0, int pageSize = int.MaxValue,
         bool showHidden = false)
    {
        var result = await _departmentRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrWhiteSpace(departmentName))
                query = query.Where(l => l.Name.Contains(departmentName));

            if (!string.IsNullOrWhiteSpace(departmentCode))
                query = query.Where(l => l.Code.Contains(departmentCode));

            if (!showHidden)
                query = query.Where(l => l.Active);
            query = query.Where(l => !l.Deleted);

            query = query.OrderByDescending(l => l.Id);

            return query;
        }, pageIndex, pageSize);
      

        return result;
    }
    public async Task<IQueryable<Department>> GetAllDepartmentsAsync(string departmentName = null,
         string departmentCode = null)
    {
        var query = from d in _departmentRepository.Table
                    where d.Deleted == false && d.Active == true && (string.IsNullOrEmpty(departmentName) || d.Name.Contains(departmentName)) && (string.IsNullOrEmpty(departmentCode) || d.Code.Contains(departmentCode))
                    select d;
        return await Task.FromResult(query);
    }

    #endregion
}

