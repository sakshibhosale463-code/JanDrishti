using Project.Core;
using Project.Core.Domain.Catalog;
using Project.Services.Common.DataModel;

namespace Project.Services.Catalog;

/// <summary>
/// Represents department service interface
/// </summary>
public partial interface IDepartmentService
{
    /// <summary>
    /// Get department select list
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the select data list
    /// </returns>
    Task<IList<SelectListDataModel>> GetSelectDataModelListAsync();

    /// <summary>
    /// Gets a department by identifier
    /// </summary>
    /// <param name="departmentId">Department identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the department
    /// </returns>
    Task<Department> GetDepartmentByIdAsync(long departmentId);

    /// <summary>
    /// Gets a departments by identifier list
    /// </summary>
    /// <param name="departmentIds">Department identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the department list
    /// </returns>
    Task<IList<Department>> GetDepartmentsByIdsAsync(IList<long> departmentIds);

    /// <summary>
    /// Check whether a department code is already exist or not
    /// </summary>
    /// <param name="code">Department code</param>
    /// <param name="departmentId">Department identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    Task<bool> HasDepartmentCodeAsync(string code, long departmentId = 0);

    /// <summary>
    /// Inserts a department
    /// </summary>
    /// <param name="department">Department</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the department
    /// </returns>
    Task InsertDepartmentAsync(Department department);

    /// <summary>
    /// Updates a department
    /// </summary>
    /// <param name="department">Department</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the department
    /// </returns>
    Task UpdateDepartmentAsync(Department department);

    /// <summary>
    /// Deletes a department
    /// </summary>
    /// <param name="department">Department</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the department
    /// </returns>
    Task DeleteDepartmentAsync(Department department);

    ///<summary>
    /// Gets all departments
    /// </summary>
    /// <param name="departmentName">Department name</param>
    /// <param name="departmentCode">Department code; null to load all records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains a paged list of departments
    /// </returns>
    Task<IPagedList<Department>> GetAllDepartmentsAsync(string departmentName = null,
         string departmentCode = null,
         int pageIndex = 0, int pageSize = int.MaxValue,
         bool showHidden = false);
    Task<IQueryable<Department>> GetAllDepartmentsAsync(string departmentName = null, string departmentCode = null);
  
}
